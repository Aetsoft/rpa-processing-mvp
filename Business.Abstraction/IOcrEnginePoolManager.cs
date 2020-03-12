using DomainModels.Enums;

namespace Business.Abstraction
{
    public interface IOcrEnginePoolManager
    {
        IOcrEngine GetEngineForLang(SupportedLangs lang);
    }
}
