namespace DNI.Core.Services
{
    using System;
    using DNI.Core.Contracts.Services;
    using Markdig;

    internal sealed class DefaultMarkdownToHtmlService : IMarkdownToHtmlService
    {
        public string ToHtml(string markdown, Action<MarkdownPipelineBuilder> builder)
        {
            var pipelineBuilder = new Markdig.MarkdownPipelineBuilder();
            builder?.Invoke(pipelineBuilder);
            return Markdig.Markdown.ToHtml(markdown, pipelineBuilder.Build());
        }
    }
}
