using System;
using UnityEngine;
using Proyecto26;

public class User
{
    public string question;
    public string answer;
    public string a_answer;
    public string b_answer;
    public string c_answer;
    public string correct_answer;
    public int total_true_false_questions;
    public int total_quiz_questions;
    public int total_circle_slot_questions;
    public float data_base_version;



    public User()
    {
        question = TrueFalseData.playerQuestion;
        answer = TrueFalseData.playerAnswer;
        total_true_false_questions = TrueFalseData.numOfTFQuestions;
        total_quiz_questions = QuizData.numOfQuizQuestions;
        correct_answer = QuizData.playerCorrectAnswer;
        a_answer = QuizData.playerAAnswer;
        b_answer = QuizData.playerBAnswer;
        c_answer = QuizData.playerCAnswer;
    }

}
