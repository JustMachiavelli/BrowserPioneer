using FluentValidation;

namespace CrawlPioneer.Application.Commands
{
    public class DownloadImageValidator : AbstractValidator<DownloadImageCommand>
    {
        public DownloadImageValidator()
        {
            RuleFor(dto => dto.PageUrl)
                .NotEmpty().WithMessage("PageUrl必填！")
                .Must(BeAValidHttpsUrl).WithMessage("PageUrl必须是https链接！");
        }

        private bool BeAValidHttpsUrl(string pageUrl)
        {
            if (Uri.TryCreate(pageUrl, UriKind.Absolute, out var uriResult))
            {
                return uriResult.Scheme == Uri.UriSchemeHttps;
            }
            return false;
        }
    }
}
