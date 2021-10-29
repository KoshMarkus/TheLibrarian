using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace TheLibrarian.Managers
{
    public class BookManager : MonoBehaviour
    {
        private bool won;
        private bool lost;
    
        [SerializeField] private int shuffleQuantity;
        public List<GameObject> bookContainers;
        public List<GameObject> chosenBookContainers;

        public UnityEvent<string> onShuffleEnd;
        public UnityEvent<string> onExchangeEnd;
        public UnityEvent onWin;
        public UnityEvent onDefeat;
    
        private int turnsRemaining;

        private bool gameOn;
    
        private void Start()
        {
            GetBooks();
            ShuffleBooks();
            gameOn = true;
        }

        private void GetBooks()
        {
            foreach (Transform shelf in transform)
            {
                foreach (Transform bookContainer in shelf)
                {
                    if (bookContainer.transform.GetChild(0).GetComponent<Book>())
                    {
                        bookContainers.Add(bookContainer.gameObject);
                    }
                }
            }
        }

        private void ShuffleBooks()
        {
            for(var i = 0; i < shuffleQuantity; i++)
            {
                //Get random book and its shelf
                int bookId = Random.Range(0, bookContainers.Count);
                Transform shelf = bookContainers[bookId].transform.parent;

                GameObject firstBook = bookContainers[bookId];
            
                ChooseBook(bookContainers[bookId]);
                bookContainers.Remove(bookContainers[bookId]);
            
                //Find other shelves
                List<Transform> otherShelves = new List<Transform>();

                foreach (Transform otherShelf in transform)
                {
                    if (otherShelf != shelf)
                    {
                        otherShelves.Add(otherShelf);
                    }
                }

                //Get shelve where are less than half of a books have a same color

                Transform shelfToUse = otherShelves[0];
            
                foreach (Transform newShelf in otherShelves)
                {
                    shelfToUse = newShelf;

                    float bookOnShelfQuantity = 0;
                    float booksOfTheSameColor = 0;
                
                    foreach (Transform book in newShelf)
                    {
                        if (book.GetChild(0).GetComponent<Book>())
                        {
                            bookOnShelfQuantity++;
                        
                            if (book.GetChild(0).GetComponent<Book>().bookColor == firstBook.transform.GetChild(0).GetComponent<Book>().bookColor)
                            {
                                booksOfTheSameColor++;
                            }
                        }
                    }

                    if (booksOfTheSameColor < Math.Floor(bookOnShelfQuantity / 2f))
                    {
                        break;
                    }
                }

                //Get random book of from that shelf

                foreach (Transform book in shelfToUse)
                {
                    if (book.GetChild(0).GetComponent<Book>())
                    {
                        if (book.GetChild(0).GetComponent<Book>().bookColor != firstBook.transform.GetChild(0).GetComponent<Book>().bookColor)
                        {
                            turnsRemaining++;
                            ChooseBook(book.gameObject);
                            bookContainers.Remove(book.gameObject);
                            break;
                        }
                    }
                }
            }
        
            onShuffleEnd.Invoke(turnsRemaining.ToString());
        }

        private void CheckIfWon()
        {
            var quantityOfShelves = 0;
            var quantityOfFullShelves = 0;
            
            //Check it full and check if all shelves are good
            foreach (Transform shelf in transform)
            {
                quantityOfShelves++;
                
                if (shelf.GetComponent<Shelf>().areBooksRight)
                {
                    quantityOfFullShelves++;
                }
            }

            if (quantityOfShelves == quantityOfFullShelves && !lost)
            {
                won = true;
                onWin.Invoke();
            }
        }
    
        public void ChooseBook(GameObject book)
        {
            chosenBookContainers.Add(book);

            if (chosenBookContainers.Count >= 2)
            {
                //Exchange shelves
            
                var firstShelf = chosenBookContainers[0].transform.parent;
                var secondShelf = chosenBookContainers[1].transform.parent;

                var tempShelf = firstShelf;
                chosenBookContainers[0].transform.parent = secondShelf;
                chosenBookContainers[1].transform.parent = tempShelf;
            
                //Exchange positions

                var firstBook = chosenBookContainers[0].transform.GetChild(0).gameObject.GetComponent<Book>();
                var secondBook = chosenBookContainers[1].transform.GetChild(0).gameObject.GetComponent<Book>();
            
                var tempPosition = firstBook.bookContainerCurrentPosition;
                firstBook.bookContainerCurrentPosition = secondBook.bookContainerCurrentPosition;
                secondBook.bookContainerCurrentPosition = tempPosition;

                //Reset book choosing state
            
                firstBook.ChangeChosen(false);
                secondBook.ChangeChosen(false);

                //Check if shelves are fully completed
            
                firstShelf.GetComponent<Shelf>().CheckBooks();
                secondShelf.GetComponent<Shelf>().CheckBooks();
            
                chosenBookContainers.Clear();

                if (gameOn)
                {
                    turnsRemaining--;
                
                    if (turnsRemaining < 0)
                    {
                        turnsRemaining = 0;
                    }
                
                    onExchangeEnd.Invoke(turnsRemaining.ToString());
                
                    CheckIfWon();
                
                    if (turnsRemaining == 0 && !won)
                    {
                        lost = true;
                        onDefeat.Invoke();
                    }
                }
            }
        }
    
    
    }
}
