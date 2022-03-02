using System;
using System.IO;
using System.Collections;
using RansacBot.Ground;

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

	public interface ITickProvider : IItemProvider<Tick>
	{
		public event Action<Tick> NewTick;
		event Action<Tick> IItemProvider<Tick>.NewItem 
		{ add => NewTick += value; remove => NewTick -= value; }
		void IItemProvider<Tick>.Subscribe(Action<Tick> action) => NewTick += action;
		void IItemProvider<Tick>.Unsubscribe(Action<Tick> action) => NewTick -= action;
	}

	public interface ITickProcessor : IItemProcessor<Tick>
	{
		void OnNewTick(Tick tick);
		Action<Tick> IItemProcessor<Tick>.Processor { get => OnNewTick; }
	}

	public interface IExtremumFilter
	{
		void OnNewExtremum(Tick extremum, VertexType vertexType, Tick current);
		public event ExtremumHandler NewExtremum;
	}
}
