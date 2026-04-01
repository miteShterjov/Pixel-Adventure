using UnityEngine;

public class TO_DO_SHEET : MonoBehaviour
{
    public string[] toDoList;

    void Start()
    {
        toDoList = new string[4];
        toDoList[1] = "Complete the flying enemies";
        toDoList[2] = "Add the missing dead zone in level 2";
        toDoList[3] = "Expand the game with more content";
    }
}
    