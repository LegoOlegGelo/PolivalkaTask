namespace PolivalkaTask
{
	public class Flower
	{
		public double X { get; private set; }
		public double Y { get; private set; }
		public string Name { get; private set; }
		
		public Flower(string name, double x, double y)
		{
			X = x;
			Y = y;
			Name = name;
		}
	}
}