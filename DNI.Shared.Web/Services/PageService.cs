using DNI.Shared.Web.Contracts;
using DNI.Shared.Web.Domains;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNI.Shared.Web.Services
{
    public class PageService : IPageService
    {
        public async Task<Page> GetPage(string pageName, int? parentPageId = null)
        {
            return await Task.FromResult(new Page
            {
                Description = "Nunc tincidunt, nibh eu euismod vehicula, massa erat tristique lacus, ut luctus felis ipsum ac metus. Pellentesque et tortor ut quam fringilla volutpat eu nec felis. Vestibulum dictum at massa nec venenatis. Cras gravida ornare est, sed ultrices libero luctus auctor. Ut dui dolor, cursus sed aliquet in, mollis vel tellus. Nunc eleifend lacus sed imperdiet dignissim. Sed vel neque sit amet felis convallis molestie fringilla vel nunc. Etiam porta risus nec tristique cursus. Pellentesque lorem lectus, maximus quis lorem eget, egestas aliquet nunc. Aenean ut sem at risus mollis vehicula. Cras at pulvinar velit.",
                Keywords = "Nunc, tincidunt, nibh, eu, euismod, vehicula, massa, erat, tristique, lacus, ut, luctus, felis, ipsum, ac, metus",
                Sections = GetSections(),
                Title = "Home",
                Id = 1
            });
        }

        public Task<Section> GetPageSection(string pageName, int sectionId, int? parentPageId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Section>> GetPageSections(int sectionId)
        {
            return await Task.FromResult(new[]{
                new Section { SectionTypeId = 3, Name="bulletPoint1", Container = "list-item", Content = "A bullet point" },
                new Section { SectionTypeId = 3, Name="bulletPoint2", Container = "list-item", Content = "Another bullet point" } });
        }

        public async Task<IEnumerable<StyleSheet>> GetStyleSheets(Page page)
        {
            return await Task.FromResult(new[] { 
                new StyleSheet { Type = "text/css", ReferenceUrl = "/Content/_normalise.css" },
                new StyleSheet { Type = "text/css", ReferenceUrl = "/Content/site.css" } 
            });
        }

        private Section[] GetSections()
        {
            return new[]
            {
                new Section
                {
                    SectionTypeId = 1,
                    Container = "summary",
                    Content = "<h1>Home</h1><p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec molestie lacus leo, in consectetur lorem hendrerit et. Donec auctor lorem velit, ut ullamcorper dui bibendum quis. Sed purus velit, accumsan id neque non, auctor egestas sapien. Praesent porta imperdiet nulla, ut fermentum elit volutpat scelerisque. Vestibulum sodales massa eget ligula dapibus, ut viverra neque viverra. Sed in condimentum lacus. Aenean elit diam, ultricies eget ullamcorper at, porta sed nisl. Etiam tristique bibendum mattis. Maecenas quis nunc venenatis, tincidunt arcu eget, volutpat nisi. Integer at turpis metus. Mauris dignissim malesuada sapien ut tristique. Cras id diam libero. Nunc tincidunt porta ex, vel accumsan nulla finibus faucibus. Etiam mi arcu, ornare vitae imperdiet placerat, accumsan at dui.</p><ul>[!bulletPoint1] [!bulletPoint2]</ul><p>Nunc tincidunt, nibh eu euismod vehicula, massa erat tristique lacus, ut luctus felis ipsum ac metus. Pellentesque et tortor ut quam fringilla volutpat eu nec felis. Vestibulum dictum at massa nec venenatis. Cras gravida ornare est, sed ultrices libero luctus auctor. Ut dui dolor, cursus sed aliquet in, mollis vel tellus. Nunc eleifend lacus sed imperdiet dignissim. Sed vel neque sit amet felis convallis molestie fringilla vel nunc. Etiam porta risus nec tristique cursus. Pellentesque lorem lectus, maximus quis lorem eget, egestas aliquet nunc. Aenean ut sem at risus mollis vehicula. Cras at pulvinar velit.</p>"
                },
                new Section
                {
                    SectionTypeId = 1,
                    Container = "feature",
                    Content = "<h2>Some useful info</h1><div class=\"flex-table\"><div class=\"row\"><div class=\"col\"><p>Donec rutrum elit convallis eros finibus, sed lobortis ligula bibendum. Nunc et mi arcu. Mauris ultrices erat tellus, tincidunt luctus dolor iaculis sed. Duis sit amet sapien elit. Nunc ac vulputate orci, non hendrerit nibh. Vestibulum posuere eget libero vitae maximus. Sed eu mollis magna. Phasellus ut convallis mi. Duis non dignissim ipsum. Vestibulum sit amet risus erat. In eget convallis urna. Donec in dolor ac ligula dapibus luctus sit amet vitae eros. Integer vitae orci fringilla, condimentum lorem sed, tincidunt sapien. Duis quis auctor risus. Cras accumsan erat ut leo efficitur dignissim. Lorem ipsum dolor sit amet, consectetur adipiscing elit.</p><p>Nam semper luctus lobortis. Cras nec volutpat erat, in sodales quam. Integer dui ipsum, varius ac sapien ac, blandit luctus erat. In ultrices feugiat eros, non finibus eros iaculis eu. Maecenas a placerat lectus, ut gravida libero. Interdum et malesuada fames ac ante ipsum primis in faucibus. Mauris faucibus semper urna in porttitor. Mauris id venenatis ligula. Integer eu nunc nec elit volutpat consectetur eget a neque. Aliquam erat volutpat. Etiam a purus tristique, ultricies metus ac, ultrices dui. Mauris dolor justo, auctor eu ante sit amet, elementum lacinia enim. Nullam pharetra id magna ut faucibus. Sed semper gravida imperdiet. Maecenas porttitor id nisl sit amet ornare.</p></div><div class=\"col\"><p>Donec rutrum elit convallis eros finibus, sed lobortis ligula bibendum. Nunc et mi arcu. Mauris ultrices erat tellus, tincidunt luctus dolor iaculis sed. Duis sit amet sapien elit. Nunc ac vulputate orci, non hendrerit nibh. Vestibulum posuere eget libero vitae maximus. Sed eu mollis magna. Phasellus ut convallis mi. Duis non dignissim ipsum. Vestibulum sit amet risus erat. In eget convallis urna. Donec in dolor ac ligula dapibus luctus sit amet vitae eros. Integer vitae orci fringilla, condimentum lorem sed, tincidunt sapien. Duis quis auctor risus. Cras accumsan erat ut leo efficitur dignissim. Lorem ipsum dolor sit amet, consectetur adipiscing elit.</p><p>Nam semper luctus lobortis. Cras nec volutpat erat, in sodales quam. Integer dui ipsum, varius ac sapien ac, blandit luctus erat. In ultrices feugiat eros, non finibus eros iaculis eu. Maecenas a placerat lectus, ut gravida libero. Interdum et malesuada fames ac ante ipsum primis in faucibus. Mauris faucibus semper urna in porttitor. Mauris id venenatis ligula. Integer eu nunc nec elit volutpat consectetur eget a neque. Aliquam erat volutpat. Etiam a purus tristique, ultricies metus ac, ultrices dui. Mauris dolor justo, auctor eu ante sit amet, elementum lacinia enim. Nullam pharetra id magna ut faucibus. Sed semper gravida imperdiet. Maecenas porttitor id nisl sit amet ornare.</p></div></div></div>"
                },
                new Section
                {
                    SectionTypeId = 1,
                    Container = "foot-note",
                    Content = "<h2>Footer</h1><p>Donec rutrum elit convallis eros finibus, sed lobortis ligula bibendum. Nunc et mi arcu. Mauris ultrices erat tellus, tincidunt luctus dolor iaculis sed. Duis sit amet sapien elit. Nunc ac vulputate orci, non hendrerit nibh. Vestibulum posuere eget libero vitae maximus. Sed eu mollis magna. Phasellus ut convallis mi. Duis non dignissim ipsum. Vestibulum sit amet risus erat. In eget convallis urna. Donec in dolor ac ligula dapibus luctus sit amet vitae eros. Integer vitae orci fringilla, condimentum lorem sed, tincidunt sapien. Duis quis auctor risus. Cras accumsan erat ut leo efficitur dignissim. Lorem ipsum dolor sit amet, consectetur adipiscing elit.</p><p>Nam semper luctus lobortis. Cras nec volutpat erat, in sodales quam. Integer dui ipsum, varius ac sapien ac, blandit luctus erat. In ultrices feugiat eros, non finibus eros iaculis eu. Maecenas a placerat lectus, ut gravida libero. Interdum et malesuada fames ac ante ipsum primis in faucibus. Mauris faucibus semper urna in porttitor. Mauris id venenatis ligula. Integer eu nunc nec elit volutpat consectetur eget a neque. Aliquam erat volutpat. Etiam a purus tristique, ultricies metus ac, ultrices dui. Mauris dolor justo, auctor eu ante sit amet, elementum lacinia enim. Nullam pharetra id magna ut faucibus. Sed semper gravida imperdiet. Maecenas porttitor id nisl sit amet ornare.</p>"
                }
            };
        }
    }
}
