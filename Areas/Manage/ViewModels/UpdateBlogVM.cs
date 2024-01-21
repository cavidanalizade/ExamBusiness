using FluentValidation;

namespace ExamBusiness.Areas.Manage.ViewModels
{
    public class UpdateBlogVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile Image { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class UpdateBlogValidatorVM : AbstractValidator<UpdateBlogVM>
    {
        public UpdateBlogValidatorVM()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title bos ola bilmez");
            RuleFor(x => x.Title).MinimumLength(3).WithMessage("Title 3-den kicik ola bilmez");
            RuleFor(x => x.Title).MaximumLength(25).WithMessage("Title 25-den boyuk ola bilmez");

            RuleFor(x => x.Description).NotEmpty().WithMessage("Description bos ola bilmez");
            RuleFor(x => x.Description).MinimumLength(3).WithMessage("Description 3-den kicik ola bilmez");
            RuleFor(x => x.Description).MaximumLength(100).WithMessage("Description 25-den boyuk ola bilmez");

            RuleFor(x => x.Image).NotEmpty().WithMessage("Image Bos ola bilmez");
        }
    }
}
