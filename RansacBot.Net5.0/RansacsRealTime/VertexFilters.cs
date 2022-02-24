using System;
using System.IO;
using System.Collections;


namespace RansacsRealTime
{
    public delegate void VertexHandler(Tick tick, VertexType vertexType);
	public delegate void ExtremumHandler(Tick extremum, VertexType vertexType, Tick current);

	public interface IVertexFinder
	{
		void OnNewTick(Tick tick);
		public event VertexHandler NewVertex; 
		public event VertexHandler LastVertexWasExtremum;
		public event ExtremumHandler NewExtremum;
	}

	public interface IVertexFilter
	{
		void OnNewVertex(Tick tick, VertexType vertexType);
		public event VertexHandler NewVertex;
	}

	public interface ITickFilter : ITickProvider, ITickProcessor { }

	public interface ITickProvider
	{
		public event Action<Tick> NewTick;
	}

	public interface ITickProcessor
	{
		void OnNewTick(Tick tick);
	}

	public interface IExtremumFilter
	{
		void OnNewExtremum(Tick extremum, VertexType vertexType, Tick current);
		public event ExtremumHandler NewExtremum;
	}
}
