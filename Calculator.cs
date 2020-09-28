using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PolivalkaTask
{
	public class Calculator
	{
		private const double SumVal = 0.1;

		private readonly double _polivalkaX;
		private readonly double _polivalkaY;
		private readonly double _polivalkaA;
		private readonly IList<Flower> _flowers;

		public Calculator(string polivalkaData, IEnumerable<string> flowersData)
		{
			var povData = polivalkaData.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

			if (povData.Length != 3)
				throw new ArgumentException();

			_polivalkaX = Convert.ToDouble(povData[0]);
			_polivalkaY = Convert.ToDouble(povData[1]);
			_polivalkaA = Convert.ToDouble(povData[2]);

			_flowers = new Collection<Flower>();

			foreach (var f in flowersData)
			{
				var fData = f.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

				if (fData.Length != 3)
					throw new ArgumentException();

				var flower = new Flower(fData[0], Convert.ToDouble(fData[1]), Convert.ToDouble(fData[2]));

				_flowers.Add(flower);
			}
		}

		public double Calc()
		{
			var maxCount = -1; // максимальное количество сортов
			var maxA = 0.0; // угол нижней границы при максимльном количестве сортов

			for (var i = 0.0; i < 360; i += Math.Round(i + SumVal, 2))
			{
				var count = CheckForA(i);

				// если находим угол, при котором количество сортов больше максимального, то записываем его
				if (count <= maxCount)
					continue;

				maxCount = count;
				maxA = i;
			}

			return maxA + _polivalkaA / 2;
		}

		private int CheckForA(in double alpha)
		{
			var flowersSorts = new List<Flower>(); // растения с уникальными сортами

			var lowLim = alpha; // нижняя граница (предел) угла обзора поливалки
			var highLim = alpha + _polivalkaA; // верхняя граница (предел) угла обзора поливалки

			foreach (var flower in _flowers)
				// если цветок входит в промежуток и его сорта еще нет в flowersSorts, то добавляем его
				if (CheckForFlower(flower, lowLim, highLim)
				    && !flowersSorts.Exists(f => f.Name == flower.Name))
					flowersSorts.Add(flower);

			return flowersSorts.Count;
		}

		private bool CheckForFlower(in Flower f, in double lowLim, in double highLim)
		{
			var dx = f.X - _polivalkaX; // позиция по x относительно поливалки
			var dy = f.Y - _polivalkaY; // позиция по y относительно поливалки

			// координатная четверть (нужна для правильного определения угла цветка относительно поливалки)
			var coordQuarter = (dx >= 0, dy >= 0) switch
			{
				(true, true) => 1,
				(false, true) => 2,
				(false, false) => 3,
				(true, false) => 4
			};

			const double x1 = 1, y1 = 0; // координаты вектора отсчета (1;0)

			var vectorMultiple = x1 * dx + y1 * dy;
			var scalarMultiple1 = Math.Sqrt(x1 * x1 + y1 * y1);
			var scalarMultiple2 = Math.Sqrt(dx * dx + dy * dy);

			var cosValue = vectorMultiple / (scalarMultiple1 * scalarMultiple2); // cos угла между векторами

			var beta = Math.Round(Math.Acos(cosValue) / Math.PI * 180, 4); // угол цветка относительно поливалки

			if (coordQuarter == 3 || coordQuarter == 4)
				beta += 180;

			return lowLim <= beta && beta <= highLim;
		}
	}
}