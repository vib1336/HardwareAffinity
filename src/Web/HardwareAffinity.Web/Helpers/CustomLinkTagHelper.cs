namespace HardwareAffinity.Web.Helpers
{
    using Microsoft.AspNetCore.Razor.TagHelpers;

    public class CustomLinkTagHelper : TagHelper
    {
        public string StarsClass { get; set; }

        public string StarsTitle { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.Attributes.SetAttribute("class", this.StarsClass);
            output.Attributes.SetAttribute("title", this.StarsTitle);
            output.Content.SetHtmlContent("<i class=\"fa fa-star\" aria-hidden=\"true\"></i>");
        }
    }
}
