import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Post } from '../post';
import { debounceTime, takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { PostService } from '../post.service';

@Component({
  selector: 'post-edit',
  templateUrl: 'post-edit.component.html',
})
export class PostEditComponent implements OnInit, OnDestroy {
  private unsubscribe$ = new Subject<void>();
  pageTitle: string;
  errorMessage: string;
  post: Post;
  editForm: FormGroup;
  tagErrors: string[];

  validationMessages = {
    required: "Don't make a fun of me! It cannot be empty.",
    maxLength: "Slow down! It's too long",
    minLength: "Make an effort! It's too short.",
    tagEmpty: "Empty tag! Look what've you done..",
    tagRepeated: "I could swear I've already seen tag like this",
  }

  isControlInvalid: { [controlName: string]: boolean } = {};

  constructor(private formBuilder: FormBuilder, private route: ActivatedRoute,
    private postService: PostService, private router: Router) { }

  ngOnInit() {
    this.resolveData();
    this.buildForm()
    this.subscribeToFormControlChanges('title');
    this.subscribeToFormControlChanges('contents');
    this.addTagWhenSpaceTyped();
    this.makeSureTagsArrayInitialized();
  }

  ngOnDestroy() {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  resolveData() {
    this.route.data.subscribe((data) => {
      this.post = data['resolvedData'].post;
      this.errorMessage = data['resolvedData'].error;
      this.setTitle();
    });
  }

  setTitle() {
    if (this.postIsEmpty()) {
      this.pageTitle = 'Add new post';
    } else {
      this.pageTitle = `Edit "${this.post.title}"`
    }
  }

  postIsEmpty() {
    return !this.post.title && !this.post.contents && !this.post.tags;
  }

  buildForm() {
    this.editForm = this.formBuilder.group({
      title: ['', [
        Validators.required,
        Validators.maxLength(50)
      ]],
      contents: ['', [
        Validators.required,
        Validators.minLength(10),
        Validators.maxLength(3000),
      ]],
      tag: ''
    });
  }

  subscribeToFormControlChanges(formControlName: string) {
    const control = this.editForm.get(formControlName);
    control.valueChanges.pipe(
      takeUntil(this.unsubscribe$),
      debounceTime(1000)
    ).subscribe(() => {
      control.updateValueAndValidity();
      this.isControlInvalid[formControlName] = this.isInvalid(control) ? true : false;
    });
  }

  addTagWhenSpaceTyped() {
    const control = this.editForm.get('tag');
    control.valueChanges.pipe(
      takeUntil(this.unsubscribe$)
    ).subscribe(data => {
      if (data[data.length - 1] === ' ') {
        this.onAddTag();
        control.setValue('');
      }
    })
  }

  makeSureTagsArrayInitialized() {
    this.tagErrors = new Array();
    if (!this.post.tags) {
      this.post.tags = new Array();
    }
  }

  onSave() {
    if (this.editForm.invalid) {
      this.updateErrorsForAllControls();
    } else {
      this.addNewPost()
    }
  }

  isInvalid(form: AbstractControl) {
    const needToHighlight = (form.touched || form.dirty) && form.invalid
    return needToHighlight ? true : false;
  }

  updateErrorsForAllControls() {
    this.isControlInvalid['title'] = true;
    this.isControlInvalid['contents'] = true;
  }

  addNewPost() {
    this.post.title = this.editForm.get('title').value;
    this.post.contents = this.editForm.get('contents').value;
    this.postService.postPost(this.post).pipe(
      takeUntil(this.unsubscribe$)
    ).subscribe(
      () => { this.router.navigate(['/posts']) },
      (error) => { console.log(error.error.errors) }),
      (complete) => console.log(complete);
  }

  onAddTag() {
    const newTag = this.getTagFromInputAndClear();
    if (newTag) {
      this.addTagOrTagError(newTag);
    } else {
      this.addTagError(this.validationMessages.tagEmpty);
    }
  }

  getTagFromInputAndClear() {
    const newTag = this.editForm.get('tag').value;
    this.editForm.get('tag').setValue('');
    return newTag;
  }

  addTagOrTagError(newTag: string) {
    if (this.tagNotAlreadyTyped(newTag)) {
      this.addNewTag(newTag);
    } else {
      this.addTagError(this.validationMessages.tagRepeated);
    }
  }

  tagNotAlreadyTyped(tagToCheck: string) {
    return !this.post.tags.find(tag => tag.trim() === tagToCheck.trim());
  }

  addNewTag(newTag: string) {
    newTag = newTag.trim();
    this.post.tags.push(newTag);
  }

  addTagError(message: string) {
    this.tagErrors.push(message);
  }

  onDeleteTag(tagToDelete: string) {
    this.post.tags = this.post.tags.filter(tag => tag !== tagToDelete);
  }

  onDeleteError(errorToDelete: string) {
    this.tagErrors = this.tagErrors.filter(error => error !== errorToDelete);
  }

  getErrorsFor(formControlName: string) {
    return this.editForm.get(formControlName).errors;
  }
}
