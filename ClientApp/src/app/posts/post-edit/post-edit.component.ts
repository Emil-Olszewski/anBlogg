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
    required: "Don't make fun of me! It cannot be empty.",
    maxLength: "Slow down! It's too long",
    minLength: "Make an effort! It's too short.",
    tagEmpty: "Empty tag! Look what've you done..",
    tagRepeated: "I could swear I already saw tag like this",
  }

  isControlInvalid: { [controlName: string]: boolean } = {};

  constructor(private formBuilder: FormBuilder, private route: ActivatedRoute,
    private postService: PostService, private router: Router) { }

  ngOnInit() {
    this.resolveData();
    this.buildForm()

    this.subscribeToFormControlChanges('title');
    this.subscribeToFormControlChanges('contents');
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
        Validators.maxLength(1000),
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
      if (this.isInvalid(control)) {
        this.isControlInvalid[formControlName] = true;
      } else {
        this.isControlInvalid[formControlName] = false;
      }
    });
  }

  isInvalid(form: AbstractControl) {
    if ((form.touched || form.dirty) && form.invalid) {
      return true;
    } else {
      return false;
    }
  }

  onSave() {
    if (this.editForm.invalid) {
      this.updateErrorsForAllControls();
    } else {
      this.post.title = this.editForm.get('title').value;
      this.post.contents = this.editForm.get('contents').value;

      this.postService.postPost('184edd78-aeab-4c20-becc-6d3dc9f1b841', this.post).pipe(
        takeUntil(this.unsubscribe$)
      ).subscribe(
        (value) => { this.router.navigate(['/posts'])}, 
        (error) => { console.log(error.error.errors) }),
        (complete) => console.log(complete);
    }
  }

  updateErrorsForAllControls() {
    this.isControlInvalid['title'] = true;
    this.isControlInvalid['contents'] = true;
  }

  onAddTag() {
    const newTag = this.editForm.get('tag').value;
    if (newTag) {
      this.post.tags = this.makeSureArrayInitialized(this.post.tags);
      if (this.tagNotAlreadyTyped(newTag)) {
        this.addTagAndResetInput(newTag);
      } else {
        this.addTagError(this.validationMessages.tagRepeated);
      }
    } else {
      this.addTagError(this.validationMessages.tagEmpty);
    }
  }

  makeSureArrayInitialized(array: any[]) {
    if (array) {
      return array;
    } else {
      return [];
    }
  }

  tagNotAlreadyTyped(tagToCheck: string) {
    return !this.post.tags.find(tag => tag === tagToCheck);
  }

  addTagAndResetInput(newTag: string) {
    this.post.tags.push(newTag);
    this.editForm.get('tag').setValue('');
  }

  addTagError(message: string) {
    this.tagErrors = this.makeSureArrayInitialized(this.tagErrors);
    console.log(this.tagErrors);
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
