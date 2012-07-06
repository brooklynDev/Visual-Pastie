namespace PastieAPI
{
    public enum Language
    {
        [FileExtension(".actionscript")]
        ActionScript = 2,

        [FileExtension(".sh")]
        Bash = 13,

        [FileExtension(".c")]
        [FileExtension(".cpp")]
        CandCPlusPlus = 7,

        Diff = 5,

        [FileExtension(".go")]
        Go = 21,

        [FileExtension(".erb")]
        HTML_ERB_Rails = 12,

        [FileExtension(".html")]
        [FileExtension(".xml")]
        [FileExtension(".cshtml")] //razor
        [FileExtension(".aspx")]
        HTML_XML = 11,

        [FileExtension(".java")]
        Java = 9,

        [FileExtension(".js")]
        Javascript = 10,

        [FileExtension(".m")]
        ObjectiveC = 1,
        Perl = 18,

        [FileExtension(".php")]
        PHP = 15,

        [FileExtension(".txt")]
        PlainText = 6,

        [FileExtension(".py")]
        Python = 16,

        [FileExtension(".rb")]
        Ruby = 3,
        RubyOnRails = 4,

        [FileExtension(".sql")]
        SQL = 14,

        [FileExtension(".yaml")]
        YAML = 19,

        [FileExtension(".cs")]
        CSharp = 20,

    }
}