using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace KeineMod.KeineModCode.Scripts;

public class KeineKeywords
{
    [CustomEnum] [KeywordProperties(AutoKeywordPosition.None)]
    public static CardKeyword Consume;

    [CustomEnum] [KeywordProperties(AutoKeywordPosition.None)]
    public static CardKeyword Create;

    [CustomEnum] [KeywordProperties(AutoKeywordPosition.None)]
    public static CardKeyword Recall;

    [CustomEnum] [KeywordProperties(AutoKeywordPosition.None)]
    public static CardKeyword Human;

    [CustomEnum] [KeywordProperties(AutoKeywordPosition.None)]
    public static CardKeyword Hakutaku;

    [CustomEnum] [KeywordProperties(AutoKeywordPosition.None)]
    public static CardKeyword Fullmoon;

    [CustomEnum] [KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword Knowledgeable;

    [CustomEnum] [KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword Sacredscroll;

    [CustomEnum] [KeywordProperties(AutoKeywordPosition.None)]
    public static CardKeyword Sacredpower;
    
    [CustomEnum] [KeywordProperties(AutoKeywordPosition.None)]
    public static CardKeyword Reincarnation;
}