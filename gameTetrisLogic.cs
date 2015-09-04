﻿// Привет! Начинаю писать тест задание. начал в Пятницу в 11:00, пока настроил гит, все дела...
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
	
	// добавляем список
	private List<Transform> _figures = new List<Transform>();
	private GameObject _pivot;
	public Transform _block;
	private int _curRotation = 0;
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
						material.color = Color.black; // кислотный цвет понадобился для фигуры. чёрный.
						cube.GetComponent<Renderer>().material = material;
						cube.transform.parent = transform;
						
					}
				}
				else if((j<_board.GetLength(1)-2)){ // левые и правые границы
					_board[i,j]=1;
					GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
					cube.transform.position = new Vector3(i,j,0);
					Material material = new Material(Shader.Find("Diffuse"));
					material.color = Color.black;
					cube.GetComponent<Renderer>().material = material;
					cube.transform.parent = transform;
				}
			}
		}
	}

	//**************
	// алгоритм генерации поля, с некоторыми изменениями, взят из тестового задания про которое я говорил. Так уж маленький чит, но надеюсь это не страшно...
	//**************

	/// <summary>
	/// Генерация фигур
	/// </summary>

	void FigureRandomSpawn(){
		int randFigure = Random.Range(0,6); //генератор рандома для спавна случайной фигуры
		int height = _board.GetLength(1)-4;
		int positionX = _board.GetLength(0)/2-1;
		//******

		//******

		if (randFigure == 0) { // не знаю как описать. 2*2, - Z. голубой
			_figures.Add(FigureClone(new Vector3(positionX, height,0),Color.cyan));
			_figures.Add(FigureClone(new Vector3(positionX+1, height,0),Color.cyan));
			_figures.Add(FigureClone(new Vector3(positionX, height+1,0),Color.cyan));
			_figures.Add(FigureClone(new Vector3(positionX-1, height+1,0),Color.cyan));

		} else if (randFigure == 1) { // не знаю как описать. 2*2, - S. Розовый
			_figures.Add(FigureClone(new Vector3(positionX, height,0),Color.magenta));
			_figures.Add(FigureClone(new Vector3(positionX-1, height,0),Color.magenta));
			_figures.Add(FigureClone(new Vector3(positionX, height+1,0),Color.magenta));
			_figures.Add(FigureClone(new Vector3(positionX+1, height+1,0),Color.magenta));

		} else if (randFigure == 2) { // т образный , - T. Красный
			_figures.Add(FigureClone(new Vector3(positionX, height,0),Color.red));
			_figures.Add(FigureClone(new Vector3(positionX-1, height,0),Color.red));
			_figures.Add(FigureClone(new Vector3(positionX+1, height,0),Color.red));
			_figures.Add(FigureClone(new Vector3(positionX, height+1,0),Color.red));

		} else if (randFigure == 3) { // прямая фигура - кубики в ряд, - I. Синий
			_figures.Add(FigureClone(new Vector3(positionX, height,0),Color.blue));
			_figures.Add(FigureClone(new Vector3(positionX, height+1,0),Color.blue));
			_figures.Add(FigureClone(new Vector3(positionX, height+2,0),Color.blue));
			_figures.Add(FigureClone(new Vector3(positionX, height+3,0),Color.blue));

		} else if (randFigure == 4) { // L образная, - L. Серый
			_figures.Add(FigureClone(new Vector3(positionX, height,0),Color.grey));
			_figures.Add(FigureClone(new Vector3(positionX-1, height,0),Color.grey));
			_figures.Add(FigureClone(new Vector3(positionX, height+1,0),Color.grey));
			_figures.Add(FigureClone(new Vector3(positionX, height+2,0),Color.grey));
		
		} else if (randFigure == 5) { // J образная, - J. Жёлтый
			_figures.Add(FigureClone(new Vector3(positionX, height,0),Color.yellow));
			_figures.Add(FigureClone(new Vector3(positionX+1, height,0),Color.yellow));
			_figures.Add(FigureClone(new Vector3(positionX, height+1,0),Color.yellow));
			_figures.Add(FigureClone(new Vector3(positionX, height+2,0),Color.yellow));
		} else if (randFigure == 6) { // квадрат, - Q. Зелёный
			_figures.Add(FigureClone(new Vector3(positionX, height,0),Color.green));
			_figures.Add(FigureClone(new Vector3(positionX+1, height,0),Color.green));
			_figures.Add(FigureClone(new Vector3(positionX, height+1,0),Color.green));
			_figures.Add(FigureClone(new Vector3(positionX+1, height+1,0),Color.green));
		}
	}

	Transform FigureClone(Vector3 pos, Color clr){
		
		Transform figBlock = (Transform)Instantiate(_block.transform, pos, Quaternion.identity) as Transform;
		figBlock.tag = "FigBlock"; // добавить в список Тэгов этот тег. Не знаю как его создать процедурно...
		figBlock.GetComponent<Renderer> ().material.color = clr;
		
		return figBlock;
	}



	void Update () {
		if(!_spawn && !_busted){// Если ничего не спавнится и если проиграли не проиграл - спавним фигуры дальше
			StartCoroutine("Wait");
			_spawn = true;
			//скидываем ротацию объекта в нулевое положение
			_curRotation = 0;
		}

	}

	IEnumerator Wait(){
		yield return new WaitForSeconds(_nextFigureTimeSpawn);
		FigureRandomSpawn();
	}

}