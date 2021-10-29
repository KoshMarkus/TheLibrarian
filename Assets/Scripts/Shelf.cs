using UnityEngine;

namespace TheLibrarian
{
    public class Shelf : MonoBehaviour
    {
        public bool areBooksRight;
        public void CheckBooks()
        {
            areBooksRight = true;
        
            Book.BookColors? firstBookColor = null;
            Book.BookColors? nextBookColor = null;

            foreach (Transform child in transform)
            {
                var book = child.GetChild(0);
            
                if (book.gameObject.GetComponent<Book>())
                {
                    var bookScript = book.gameObject.GetComponent<Book>();
                
                    if (firstBookColor is null)
                    {
                        firstBookColor = bookScript.bookColor;
                    }
                    else
                    {
                        nextBookColor = bookScript.bookColor;

                        if (firstBookColor != nextBookColor)
                        {
                            areBooksRight = false;
                        }
                    }
                }
            }
        }
    }
}
