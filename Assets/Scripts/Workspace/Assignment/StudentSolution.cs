using UnityEngine;
using Debug = AssignmentSystem.Services.AssignmentDebugConsole;
using System;
using NUnit.Framework;
using System.Collections.Generic;

namespace Assignment
{
    public class StudentSolution : IAssignment
    {
        public void EXAM01_FindWords(string[,] board, string[] words)
        {
            int m = board.GetLength(0);
            int n = board.GetLength(1);
            int[,] directions = new int[,]
            {
                {1,0}, {-1,0}, {0,1}, {0,-1},
                {1,1}, {1,-1}, {-1,1}, {-1,-1}
            };

            List<string> foundWords = new List<string>();

            foreach (string word in words)
            {
                bool found = false;
                for (int i = 0; i < m && !found; i++)
                {
                    for (int j = 0; j < n && !found; j++)
                    {
                        if (DFS(board, word, 0, i, j, new bool[m, n], directions))
                        {
                            foundWords.Add(word);
                            found = true;
                        }
                    }
                }
            }

            // Log ทีละคำ → Test case จะอ่านถูก
            foreach (string w in foundWords)
            {
                Debug.Log(w);
            }
        }

        // Recursive DFS สำหรับค้นหาคำ
        private bool DFS(string[,] board, string word, int index, int row, int col, bool[,] visited, int[,] directions)
        {
            if (index == word.Length) return true; // เจอคำทั้งหมดแล้ว

            int m = board.GetLength(0);
            int n = board.GetLength(1);

            // ออกนอกขอบเขต / เคยใช้แล้ว / ตัวอักษรไม่ตรง
            if (row < 0 || row >= m || col < 0 || col >= n
                || visited[row, col]
                || board[row, col] != word[index].ToString())  // แก้ตรงนี้
            {
                return false;
            }

            visited[row, col] = true;

            for (int d = 0; d < directions.GetLength(0); d++)
            {
                int newRow = row + directions[d, 0];
                int newCol = col + directions[d, 1];

                if (DFS(board, word, index + 1, newRow, newCol, visited, directions))
                {
                    visited[row, col] = false; // backtrack
                    return true;
                }
            }

            visited[row, col] = false; // backtrack
            return false;
        }

        // Recursive ในรูปแบบของอาจารย์
        /*public void EXAM01_FindWords(string[,] board, string[] words)
        {
            var foundWords = new List<string>();
            var visited = new bool[board.GetLength(0), board.GetLength(1)];
            var directions = new (int, int)[]
            {
                (-1, -1), (-1, 0), (-1, 1),
                (0, -1),          (0, 1),
                (1, -1),  (1, 0),  (1, 1)
            };

            foreach (var word in words)
            {
                bool wordFound = false;
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    for (int j = 0; j < board.GetLength(1); j++)
                    {
                        if (board[i, j] == word[0].ToString())
                        {
                            Array.Clear(visited, 0, visited.Length);
                            if (DFS(board, word, i, j, 0, visited, directions))
                            {
                                foundWords.Add(word);
                                wordFound = true;
                                break;
                            }
                        }
                    }
                    if (wordFound) break;
                }
            }

            foreach (var word in foundWords)
            {
                Debug.Log(word);
            }
        }

        private bool DFS(string[,] board, string word, int x, int y, int index, bool[,] visited, (int, int)[] directions)
        {
            // Base case: if we've matched all characters
            if (index == word.Length - 1)
            {
                return true;
            }

            // Mark current cell as visited
            visited[x, y] = true;

            // Explore all 8 directions
            foreach (var (dx, dy) in directions)
            {
                int nx = x + dx;
                int ny = y + dy;

                // Check bounds
                if (nx < 0 || nx >= board.GetLength(0) || ny < 0 || ny >= board.GetLength(1))
                {
                    continue;
                }

                // Check if already visited
                if (visited[nx, ny])
                {
                    continue;
                }

                // Check if next character matches
                if (board[nx, ny] == word[index + 1].ToString())
                {
                    // Recursively search for the rest of the word
                    if (DFS(board, word, nx, ny, index + 1, visited, directions))
                    {
                        visited[x, y] = false; // Backtrack
                        return true;
                    }
                }
            }

            // Backtrack: unmark current cell
            visited[x, y] = false;
            return false;
        }
        */
    }
}