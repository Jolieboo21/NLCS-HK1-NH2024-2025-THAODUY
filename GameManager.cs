using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject[] Tetrominos;
    public float movementFrequency = 0.8f;
    private float passedTime = 0;
    private GameObject currentTetromino;
    public TextMeshProUGUI scoreText; // Drag your ScoreText UI here
    private int score = 0;

    //public GameObject[] tetrominoPrefabs;
    //private GameObject nextTetromino;
    public GameObject gameOverText;
          public Transform nextTetrominoHolder;


    // Start is called before the first frame update
    void Start()
    {
        SpawnTetromino();
        //SpawnNextTetromino();
        gameOverText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<GridScript>().IsGameOver())
        {
            GameOver();  // Gọi hàm GameOver nếu game kết thúc
            return;  // Dừng các cập nhật khác trong Update khi game over
        }


        passedTime += Time.deltaTime;
        if (passedTime >= movementFrequency) {
            passedTime -= movementFrequency;
            MoveTetromino(Vector3.down);
        }
        UserInput();
    }
    void GameOver()
    {
        Debug.Log("Game Over");
        gameOverText.SetActive(true);  // Hiện thông báo Game Over trên màn hình
        Time.timeScale = 0;  // Dừng game
    }
    //void DisplayNextTetromino()
    //{
    //    if (nextTetromino != null)
    //    {
    //        // Xóa bỏ khối Tetris trước đó nếu cần
    //        foreach (Transform child in nextTetrominoHolder)
    //        {
    //            Destroy(child.gameObject);
    //        }

    //        // Tạo bản sao của nextTetromino để hiển thị
    //        GameObject nextTetrominoPreview = Instantiate(nextTetromino, nextTetrominoHolder);
    //        nextTetrominoPreview.transform.localPosition = Vector3.zero; // Đặt vị trí giữa Holder
    //        //nextTetrominoPreview.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); // Điều chỉnh tỷ lệ nếu cần
    //    }
    //}

    void UserInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveTetromino(Vector3.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveTetromino(Vector3.right);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentTetromino.transform.Rotate(0, 0, 90);
            if (!IsValidPosition())
            {
                currentTetromino.transform.Rotate(0, 0, -90);
            }
            else
            {
                // Cập nhật vị trí của từng phần tử con
                UpdateTetrominoPositions();
            }
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            movementFrequency = 0.2f;
        }else
        {
            movementFrequency = 0.8f;
        }
    }
    void UpdateTetrominoPositions()
    {
        foreach (Transform mino in currentTetromino.transform)
        {
            Vector2 pos = GridScript.Round(mino.position);
            mino.position = new Vector3(pos.x, pos.y, mino.position.z);
        }
    }
    void SpawnTetromino()
    {
        int index = Random.Range(0, Tetrominos.Length);
        currentTetromino = Instantiate(Tetrominos[index], new Vector3(5, 18, 0), Quaternion.identity);
    }
    //void SpawnTetromino()
    //{
    //    int index = Random.Range(0, Tetrominos.Length);
    //    Vector3 spawnPosition = new Vector3(width / 2, height - 2, 0); // Điều chỉnh để gần trên cùng grid

    //    currentTetromino = Instantiate(Tetrominos[index], spawnPosition, Quaternion.identity);

    //    // Kiểm tra nếu vị trí spawn không hợp lệ, di chuyển khối xuống cho đến khi hợp lệ
    //    while (!IsValidPosition())
    //    {
    //        currentTetromino.transform.position += Vector3.down;
    //    }
    //}

    //void SpawnNextTetromino()
    //{
    //    if (nextTetromino == null)
    //    {
    //        nextTetromino = Instantiate(tetrominoPrefabs[Random.Range(0, tetrominoPrefabs.Length)]);
    //        nextTetromino.SetActive(false);  // Ẩn khối tiếp theo
    //    }

    //    // Đưa nextTetromino vào lưới và hiển thị nó
    //    currentTetromino = nextTetromino;
    //    currentTetromino.SetActive(true);

    //    // Tạo khối tiếp theo mới và ẩn nó
    //    nextTetromino = Instantiate(tetrominoPrefabs[Random.Range(0, tetrominoPrefabs.Length)]);
    //    nextTetromino.SetActive(false);

    //    // Gọi hàm hiển thị nextTetromino
    //    DisplayNextTetromino();
    //}
    void MoveTetromino(Vector3 direction)
    {
       currentTetromino.transform.position += direction;     
        if (!IsValidPosition())
        {
            currentTetromino.transform.position -= direction;
            if(direction == Vector3.down)
            {
                GetComponent<GridScript>().UpdateGrid(currentTetromino.transform);
                CheckForLines();
                SpawnTetromino();
            }
        }
    }

    bool IsValidPosition()
    {
       return GetComponent<GridScript>().IsValidPosition(currentTetromino.transform);

    }
    void CheckForLines()
    {
        int linesCleared = GetComponent<GridScript>().CheckForLines(); // Update this to return the number of cleared lines
        if (linesCleared > 0)
        {
            UpdateScore(linesCleared);
        }
    }
    void UpdateScore(int linesCleared)
    {
        int pointsEarned = linesCleared * 10;
        score += pointsEarned;
        scoreText.text = "Score:\n" + score;
    }
}
