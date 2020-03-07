using System;
using Markdig;

namespace DNI.Core.Contracts.Services
{
    public interface IMarkdownToHtmlService
    {
        string ToHtml(string markdown, Action<MarkdownPipelineBuilder> builder = null);
    }
}
