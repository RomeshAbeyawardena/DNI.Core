using DNI.Shared.Contracts.Services;
using System;
using Markdig;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Services
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
