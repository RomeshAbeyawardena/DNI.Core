using DNI.Core.Contracts.Services;
using System;
using Markdig;

namespace DNI.Core.Services
{
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
