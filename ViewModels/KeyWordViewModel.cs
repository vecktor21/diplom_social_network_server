using server.Models;

namespace server.ViewModels
{
    public class KeyWordViewModel
    {
        public int KeyWordId { get; set; }
        public string KeyWordRu { get; set; }
        public string KeyWordEn { get; set; }
        public KeyWordViewModel(KeyWord keyWord)
        {
            this.KeyWordId = keyWord.KeyWordId;
            this.KeyWordRu = keyWord.KeyWordRu;
            this.KeyWordEn = keyWord.KeyWordEn;
        }
    }
}
