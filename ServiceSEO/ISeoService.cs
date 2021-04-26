using ModelSEO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceSEO
{
    public interface ISeoService
    {
        bool AddSeoContent(SeoContent seoContent);
        IList<SeoContent> UpdateSeoContent(List<SeoContent> tasks);
        IList<SeoContent> DeleteSeoContent(List<SeoContent> tasks);
        List<SeoContent> GetSeoContent(string QueryConditionPartParam);
    }
}
