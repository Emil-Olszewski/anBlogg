using anBlogg.Domain.Entities;
using System;

namespace anBlogg.Infrastructure.Persistence
{
    public static class BlogContextSeeds
    {
        public static Author[] Authors =
        {
            new Author()
            {
                Id = Guid.Parse("591d79d0-7742-4f9d-b285-c30319036ec0"),
                DisplayName = "Juri"
            },
            new Author()
            {
                Id = Guid.Parse("0a6c5317-a564-4e48-8e79-df271e9e72d3"),
                DisplayName = "Vanessa"
            },
            new Author()
            {
                Id = Guid.Parse("184edd78-aeab-4c20-becc-6d3dc9f1b841"),
                DisplayName = "Matthew"
            }
        };

        public static Post[] Posts =
        {
            new Post()
            {
                Id = Guid.Parse("991861b0-e24f-40d9-88f4-8567de578668"),
                AuthorId = Guid.Parse("591d79d0-7742-4f9d-b285-c30319036ec0"),
                Title = "Is PiS destroying Poland?",
                Contents = @"<p>Donec nec nisi egestas, mollis orci quis, sodales est. Fusce sed luctus diam. Sed posuere egestas arcu, vitae iaculis enim pulvinar vitae. Praesent efficitur purus nisi, vitae feugiat erat faucibus et. Donec vitae magna eu ipsum dictum dignissim aliquet sodales arcu. Sed eu ex auctor, condimentum ex ut, aliquam lectus. Praesent sollicitudin nibh vitae erat aliquet aliquet. Duis porta pharetra augue, commodo lobortis ligula placerat ut. Vivamus auctor facilisis felis vitae consequat. Vivamus dui sem, rhoncus nec condimentum id, porta ac velit. Donec pulvinar, diam quis ultricies pellentesque, purus ligula lobortis ipsum, quis consectetur erat diam sodales urna. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Maecenas elit eros, pretium eget dui et, tristique ornare ex.</p>"
            },
            new Post()
            {
                Id = Guid.Parse("1c499376-e8e3-41e9-b545-8a26d1fee602"),
                AuthorId = Guid.Parse("591d79d0-7742-4f9d-b285-c30319036ec0"),
                Title = "Is opposition defensing the law?",
                Contents = @"<p>Donec nec nisi egestas, mollis orci quis, sodales est. Fusce sed luctus diam. Sed posuere egestas arcu, vitae iaculis enim pulvinar vitae. Praesent efficitur purus nisi,  vitae feugiat erat faucibus et.</p>"
            },
            new Post()
            {
                Id = Guid.Parse("3eabbb2c-4bbd-4863-9392-674b28aa7dcb"),
                AuthorId = Guid.Parse("0a6c5317-a564-4e48-8e79-df271e9e72d3"),
                Title = "What after Corona?",
                Contents = @"<p>Donec nec nisi egestas, mollis orci quis, sodales est. Fusce sed luctus diam. Sed posuere egestas arcu, vitae iaculis enim pulvinar vitae. Praesent efficitur purus nisi, vitae feugiat erat faucibus et.</p>"
            },
            new Post()
            {
                Id = Guid.Parse("809971e5-e6d1-4d4a-8bf3-959411ecd2f7"),
                AuthorId = Guid.Parse("184edd78-aeab-4c20-becc-6d3dc9f1b841"),
                Title = "Kebab officially became the best food in the world!",
                Contents = @"<p>Vivamus auctor facilisis Praesent efficitur purus nisi vitae iaculis enim pulvinar vitae.felis vitae consequat. Vivamus dui sem, rhoncus nec condimentum id, porta ac velit. Donec pulvinar, diam quis ultricies pellentesque, purus ligula lobortis ipsum, quis consectetur erat diam sodales urna. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Maecenas elit eros, pretium eget dui et, tristique ornare ex.</p>"
            }
        };

        public static dynamic[] Tags =
        {
            new
            {
                PostId = Guid.Parse("991861b0-e24f-40d9-88f4-8567de578668"),
                Raw = "<tag><other-tag>"
            },
            new
            {
                PostId = Guid.Parse("1c499376-e8e3-41e9-b545-8a26d1fee602"),
                Raw = "<tag><some-other-tag>"
            },
            new
            {
                PostId = Guid.Parse("3eabbb2c-4bbd-4863-9392-674b28aa7dcb"),
                Raw = "<tag><other-tag>"
            },
            new
            {
                PostId = Guid.Parse("809971e5-e6d1-4d4a-8bf3-959411ecd2f7"),
                Raw = "<tag><completely-other-tag>"
            }
        };

        public static Comment[] Comments =
        {
            new Comment()
            {   
                Id = Guid.Parse("b56a3616-6b3f-4e78-aae1-04ca960c1a54"),
                PostId = Guid.Parse("3eabbb2c-4bbd-4863-9392-674b28aa7dcb"),
                AuthorId = Guid.Parse("184edd78-aeab-4c20-becc-6d3dc9f1b841"),
                Contents = "<p>My first comment! :D</p>"
            },
            new Comment()
            {
                Id = Guid.Parse("0fac4488-8943-11ea-bc55-0242ac130003"),
                PostId = Guid.Parse("3eabbb2c-4bbd-4863-9392-674b28aa7dcb"),
                AuthorId = Guid.Parse("591d79d0-7742-4f9d-b285-c30319036ec0"),
                Contents = "<p>Nice work Vanessa !!!</p>"
            }
        };
    }
}