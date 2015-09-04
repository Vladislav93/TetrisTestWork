// Привет! Начинаю писать тест задание. начал в Пятницу в 11:00, пока настроил гит, все дела...
// Программа будет делиться на несколько пунктов:
// 1) Генерация доски по правилам тетриса 10х20
// 2) Создание формы из кубиков
// 3) Движение формы внизу
// 4) Входные данные получаемые от пользователя
// 5) Создания коллизии между блоками и контурами карты (доски, игрового поля)
// 6) Вращение блоков
// 7) Проверка на заполненнсоть строки, если строка заполнена - удаляем кубики и сдвиагаем всё вниз
// 8) Проверка на "Потрачено", если игрок не справился и проиграл
// 9) Добавление интерфейса
//
//*************************************************************************************************
//
// Буду заливать по мере добавления методов. Реализацию планирую сделать максимально возможно кодом.

using UnityEngine;
using System.Collections;
using System.Collections.Generic; // понадобится дженерик

public class gameTetrisLogic : MonoBehaviour {
	// Начинаем объявлять переменные. Нам понадобится несколько.
	
	//массив рабочего поля "доски"
	public int[,] _board;
	
	//флаг который разрешает "спавн" новой фигуры
	public bool _spawn;
	
	//Секунды до спавна следующей фигуры
	public float _nextFigureTimeSpawn;
	
	//Скорость падения блоков
	public float _figureSpeed;
	
	//Уровень по высоте для "потрачено". когда игрок проигрывает.
	public int _bustedHeight = 22; //20 клеток игрвое поле + граница сверху и снизу
	
	//Переходит в "true", если игра закончилась
	private bool _busted;
	
	
	
	// остальные переменные будем добавлять по мере надобности. Это что на вскидку пришло в голову.
	
	void Start () {
		// определим массив досик игрового поля, создаём игрвое поле

		_board = new int[12,24]; // так как у нас есть границы сверху и сниузу + место для спавна фигур.
		CreateGameAreaBoard();//Генерируем игровое поле

	}
	/// <summary>
	/// Создание игровой "доски" - Полем с границами, имеющие коллидер
	/// </summary>
	void CreateGameAreaBoard(){
	
		for(int i=0; i<_board.GetLength(0);i++){
			for(int j=0; j<_board.GetLength(1);j++){
				if(i<11 && i>0){
					if(j>0 && j<_board.GetLength(1)-2){
						//генерация задней части доски. которая является фоном
						_board[i,j]=0;
						GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube); // создаём обычный приметив юнити - куб
						cube.transform.position = new Vector3(i,j,1); // перемещаем его в позицию заданную точку, проходя по матрице. чуть утапливаем вглубь
						Material material = new Material(Shader.Find("Diffuse")); // создаём новый материал со стандартным шейдером
						material.color = Color.white; // цвет материала указываем
						cube.GetComponent<Renderer>().material = material; // присваеваем нашему объекту материал новы
						cube.transform.parent = transform; // делаем под родительский объект
					}
					else if(j<_board.GetLength(1)-2){ // рисуем нижнюю границу
						_board[i,j]=1;
						GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
						cube.transform.position = new Vector3(i,j,0);
						Material material = new Material(Shader.Find("Diffuse"));
						material.color = Color.magenta; // цвет от балды. пусть немного поболят глаза. чёрный лучше, но скучнее. Я не дизайнер)
						cube.GetComponent<Renderer>().material = material;
						cube.transform.parent = transform;
						
					}
				}
				else if((j<_board.GetLength(1)-2)){ // левые и правые границы
					_board[i,j]=1;
					GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
					cube.transform.position = new Vector3(i,j,0);
					Material material = new Material(Shader.Find("Diffuse"));
					material.color = Color.magenta;
					cube.GetComponent<Renderer>().material = material;
					cube.transform.parent = transform;
				}
			}
		}
	}

	//**************
	// алгоритм генерации поля, с некоторыми изменениями, взят из тестового задания про которое я говорил. Так уж маленький чит, но надеюсь это не страшно...
	//**************

	// Генерация фигур

	void FigureRandomSpawn(){
		int randFigure = Random.Range(0,6); //генератор рандома для спавна случайной фигуры

		//******
		// информация с сайта 
		//http://mech.math.msu.su/~shvetz/54/inf/perl-problems/chTetris.xhtml
		//******

		if (randFigure == 0) { // не знаю как описать. 2*2, - Z. голубой
		
		} else if (randFigure == 1) { // не знаю как описать. 2*2, - S. Розовый
		
		} else if (randFigure == 2) { // т образный , - T. Красный
		
		} else if (randFigure == 3) { // прямая фигура - кубики в ряд, - I. Синий
		
		} else if (randFigure == 4) { // L образная, - L. Серый
		
		} else if (randFigure == 5) { // J образная, - J. Жёлтый

		} else if (randFigure == 6) { // квадрат, - Q. Зелёный
		
		}
	}

	void Update () {
		
	}
}