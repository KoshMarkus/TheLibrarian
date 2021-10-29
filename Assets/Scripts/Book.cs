using UnityEngine;
using UnityEngine.Events;

namespace TheLibrarian
{
    public class Book : MonoBehaviour
    {
        public Vector3 bookContainerCurrentPosition;
    
        [SerializeField] int speed = 20;
    
        private Animator anim;
        private bool chosen;
    
        public UnityEvent onClick;
    
        public enum BookColors
        {
            Green,
            Red,
            Blue,
            Yellow,
            Cyan
        }

        public BookColors bookColor;

        private void Awake()
        {
            bookContainerCurrentPosition = transform.position;
            anim = gameObject.GetComponent<Animator>();
        }
    
        private void Update()
        {
            var bookContainer = transform.parent;
            bookContainer.position = Vector3.MoveTowards(bookContainer.position, bookContainerCurrentPosition, Time.deltaTime * speed);
        }

        private void OnMouseOver()
        {
            anim.SetBool("hovering", true);

            if (Input.GetMouseButtonUp(0))
            {
                ChangeChosen(true);
                onClick.Invoke();
            }
        }

        private void OnMouseExit()
        {
            anim.SetBool("hovering", false);
        }

        public void ChangeChosen(bool value)
        {
            anim.SetBool("chosen", value);
            chosen = value;
        }
    }
}
