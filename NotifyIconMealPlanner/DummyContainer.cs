using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotifyIconMealPlanner
{
	class DummyContainer : IContainer
	{
		ComponentCollection _components;

		public DummyContainer()
		{
			_components = new ComponentCollection( new IComponent[] { } );
		}

		public void Add( IComponent component )
		{ }

		public void Add( IComponent component, string Name )
		{ }

		public void Remove( IComponent component )
		{ }

		public ComponentCollection Components
		{
			get { return _components; }
		}

		public void Dispose()
		{
			_components = null;
		}
	}
}
