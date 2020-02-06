using System;
using Markdig;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts.Services
{
    public interface IMarkdownToHtmlService
    {
        string ToHtml(string markdown, Action<MarkdownPipelineBuilder> builder = null);
    }
}
