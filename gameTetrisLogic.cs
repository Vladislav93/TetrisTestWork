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
	public float _nextFigureTimeSpawn=0.5f;
	
	//Скорость падения блоков
	public float _figureSpeed = 0.5f;
	
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
		InvokeRepeating("moveDown",_figureSpeed,_figureSpeed); // 

	}
	IEnumerator Wait(){
		yield return new WaitForSeconds(_nextFigureTimeSpawn);
		FigureRandomSpawn();
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

	void moveDown(){

		if(_figures.Count!=4){ //  определяем спавн позиции. проверяем, четыре ли куба в списке спавняющейся фигуры
			return;
		}
		Vector3 vec1 = _figures[0].transform.position;
		Vector3 vec2 = _figures[1].transform.position; 
		Vector3 vec3 = _figures[2].transform.position;
		Vector3 vec4 = _figures[3].transform.position;
		
		if(CheckMove(vec1,vec2,vec3,vec4)==true){    // Проверяем на дальнейшее движение

			vec1 = new Vector3(Mathf.RoundToInt(vec1.x),Mathf.RoundToInt(vec1.y-1.0f),vec1.z);
			vec2 = new Vector3(Mathf.RoundToInt(vec2.x),Mathf.RoundToInt(vec2.y-1.0f),vec2.z);
			vec3 = new Vector3(Mathf.RoundToInt(vec3.x),Mathf.RoundToInt(vec3.y-1.0f),vec3.z);
			vec4 = new Vector3(Mathf.RoundToInt(vec4.x),Mathf.RoundToInt(vec4.y-1.0f),vec4.z);
			

			_figures[0].transform.position = vec1;
			_figures[1].transform.position = vec2; 
			_figures[2].transform.position = vec3; 
			_figures[3].transform.position = vec4; 
			
		}
		else{
			//Блок во что-то врезался 
			
			

			//записываем идентификаторы для обноружения столкновения
			_board[Mathf.RoundToInt(vec1.x),Mathf.RoundToInt(vec1.y)]=1;
			_board[Mathf.RoundToInt(vec2.x),Mathf.RoundToInt(vec2.y)]=1;
			_board[Mathf.RoundToInt(vec3.x),Mathf.RoundToInt(vec3.y)]=1;
			_board[Mathf.RoundToInt(vec4.x),Mathf.RoundToInt(vec4.y)]=1;
			
			_figures.Clear(); //очищаем список
			_spawn = false; //готовы отспавнить другой блок
			
			
		}
	}
	
	
	bool CheckMove(Vector3 vec1, Vector3 vec2, Vector3 vec3, Vector3 vec4){
		//если у нас в массиве "1" - значит во что-то фрезалась фигура
		if(_board[Mathf.RoundToInt(vec1.x),Mathf.RoundToInt(vec1.y-1)]==1){
			return false;
		}
		if(_board[Mathf.RoundToInt(vec2.x),Mathf.RoundToInt(vec2.y-1)]==1){
			return false;
		}
		if(_board[Mathf.RoundToInt(vec3.x),Mathf.RoundToInt(vec3.y-1)]==1){
			return false;
		}
		if(_board[Mathf.RoundToInt(vec4.x),Mathf.RoundToInt(vec4.y-1)]==1){
			return false;
		}
		
		return true;
		
	}

	void Update () {
		if (_spawn && _figures.Count == 4) { //проверяем "блок ли это", количество кубов = 4
			
			//получаем позицию спавна объектов
			Vector3 vec1 = _figures [0].transform.position;
			Vector3 vec2 = _figures [1].transform.position; 
			Vector3 vec3 = _figures [2].transform.position;
			Vector3 vec4 = _figures [3].transform.position;
			
			
			if (Input.GetKeyDown (KeyCode.LeftArrow)) {//движение влево
				if (CheckUserMove (true, vec1, vec2, vec3, vec4)) {//Можем ли совершить движение?
					vec1.x -= 1;
					vec2.x -= 1;
					vec3.x -= 1;
					vec4.x -= 1;

					_figures [0].transform.position = vec1;
					_figures [1].transform.position = vec2; 
					_figures [2].transform.position = vec3; 
					_figures [3].transform.position = vec4; 
				}
				
				
				
			}
			if (Input.GetKeyDown (KeyCode.RightArrow)) {//движние вправо
				if (CheckUserMove ( false, vec1, vec2, vec3, vec4)) {
					vec1.x += 1;
					vec2.x += 1;
					vec3.x += 1;
					vec4.x += 1;
					

					_figures [0].transform.position = vec1;
					_figures [1].transform.position = vec2; 
					_figures [2].transform.position = vec3; 
					_figures [3].transform.position = vec4; 
					
					
				}
			}
		}
			
			if (Input.GetKey (KeyCode.DownArrow)) {
			//быстро отправляем фигуру вниз
			moveDown ();
		}
				if (!_spawn && !_busted) {// Если ничего не спавнится и если проиграли не проиграл - спавним фигуры дальше
					StartCoroutine ("Wait");
					_spawn = true;
					//скидываем ротацию объекта в нулевое положение
					_curRotation = 0;
				}

			}
		

	bool CheckUserMove( bool flag, Vector3 vec1, Vector3 vec2, Vector3 vec3, Vector3 vec4){
		//можем ли мы двигаться, не произойдёт ли столкновения (по ячейкам ==  "1")
		if(flag){//проверка влево 
			if(_board[Mathf.RoundToInt(vec1.x-1),Mathf.RoundToInt(vec1.y)]==1 || _board[Mathf.RoundToInt(vec2.x-1),Mathf.RoundToInt(vec2.y)]==1 || _board[Mathf.RoundToInt(vec3.x-1),Mathf.RoundToInt(vec3.y)]==1 || _board[Mathf.RoundToInt(vec4.x-1),Mathf.RoundToInt(vec4.y)]==1){
				return false;
			}
		}
		else{//проверка вправо
			if(_board[Mathf.RoundToInt(vec1.x+1),Mathf.RoundToInt(vec1.y)]==1 || _board[Mathf.RoundToInt(vec2.x+1),Mathf.RoundToInt(vec2.y)]==1 || _board[Mathf.RoundToInt(vec3.x+1),Mathf.RoundToInt(vec3.y)]==1 || _board[Mathf.RoundToInt(vec4.x+1),Mathf.RoundToInt(vec4.y)]==1){
				return false;
			}
		}
		return true;
	}


}