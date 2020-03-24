namespace DNI.Core.Contracts.Services
{
    using System;
    using Markdig;

    public interface IMarkdownToHtmlService
    {
        string ToHtml(string markdown, Action<MarkdownPipelineBuilder> builder = null);
    }
}
