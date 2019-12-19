using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModels.Enums;

namespace Business.Abstraction
{
    public interface IOcrEnginePoolManager
    {
        IOcrEngine GetEngineForLang(SupportedLangs lang);
    }
}
