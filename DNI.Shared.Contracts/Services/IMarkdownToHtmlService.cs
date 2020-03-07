using System;
using Markdig;

namespace DNI.Shared.Contracts.Services
{
    public interface IMarkdownToHtmlService
    {
        string ToHtml(string markdown, Action<MarkdownPipelineBuilder> builder = null);
    }
}
