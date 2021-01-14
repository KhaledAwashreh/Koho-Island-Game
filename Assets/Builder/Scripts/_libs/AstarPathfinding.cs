using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//https://github.com/mclift/SimpleAStarExample/blob/master/SimpleAStarExample/Node.cs

public enum NodeState {
	Untested,
	Open,
	Closed
}
	

public class SearchParameters {
	public Vector2 startLocation { get; set; }
	public Vector2 endLocation { get; set; }
	public bool[,] map { get; set; }

	public SearchParameters(Vector2 startLocation, Vector2 endLocation, bool[,] map){
		this.startLocation = startLocation;
		this.endLocation = endLocation;
		this.map = map;
	}
}

public class Node{
	private Node _parentNode;

	public Vector2 location { get; private set; }

	//True when the node may be traversed, otherwise false
	public bool isWalkable { get; set; }


	// Cost from start to here
	public float G { get; private set; }

	// Estimated cost from here to end
	public float H { get; private set; }

	// Flags whether the node is open, closed or untested by the PathFinder
	public NodeState State { get; set; }

	// Estimated total cost (F = G + H)
	public float F {
		get { return this.G + this.H; }
	}

	// Gets or sets the parent node. The start node's parent is always null.
	public Node parentNode {
		get { return this._parentNode; }
		set
		{
			// When setting the parent, also calculate the traversal cost from the start node to here (the 'G' value)
			this._parentNode = value;
			this.G = this._parentNode.G + GetTraversalCost(this.location, this._parentNode.location);
		}
	}

	public Node(int x, int y, bool isWalkable, Vector2 endLocation){
		this.location = new Vector2(x, y);
		this.State = NodeState.Untested;
		this.isWalkable = isWalkable;
		this.H = GetTraversalCost(this.location, endLocation);
		this.G = 0;
	}


	// Gets the distance between two points
	internal static float GetTraversalCost(Vector2 location, Vector2 otherLocation){
		float deltaX = otherLocation.x - location.x;
		float deltaY = otherLocation.y - location.y;
		return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
	}
}

public class PathFinder{
	private int width;
	private int height;
	private Node[,] nodes;
	private Node startNode;
	private Node endNode;
	private SearchParameters searchParameters;

	public PathFinder(SearchParameters searchParameters){
		this.searchParameters = searchParameters;
		InitializeNodes(searchParameters.map);
		this.startNode = this.nodes[(int)searchParameters.startLocation.x, (int)searchParameters.startLocation.y];
		this.startNode.State = NodeState.Open;
		this.endNode = this.nodes[(int)searchParameters.endLocation.x, (int)searchParameters.endLocation.y];
	}


	public List<Vector2> FindPath(){
		List<Vector2> path = new List<Vector2>();
		bool success = Search(startNode);
		if (success){
			// If a path was found, follow the parents from the end node to build a list of locations
			Node node = this.endNode;
			while (node.parentNode != null){
				path.Add(node.location);
				node = node.parentNode;
			}

			// Reverse the list so it's in the correct order when returned
			path.Reverse();
		}

		return path;
	}


	private void InitializeNodes(bool[,] map) {
		this.width = map.GetLength(0);
		this.height = map.GetLength(1);
		this.nodes = new Node[this.width, this.height];
		for (int y = 0; y < this.height; y++){
			for (int x = 0; x < this.width; x++){
				this.nodes[x, y] = new Node(x, y, map[x, y], this.searchParameters.endLocation);
			}
		}
	}

	private bool Search(Node currentNode) {
		// Set the current node to Closed since it cannot be traversed more than once
		currentNode.State = NodeState.Closed;
		List<Node> nextNodes = GetAdjacentWalkableNodes(currentNode);

		// Sort by F-value so that the shortest possible routes are considered first
		nextNodes.Sort((node1, node2) => node1.F.CompareTo(node2.F));
		foreach (var nextNode in nextNodes)
		{
			// Check whether the end node has been reached
			if (nextNode.location == this.endNode.location)
			{
				return true;
			}
			else
			{
				// If not, check the next set of nodes
				if (Search(nextNode)) // Note: Recurses back into Search(Node)
					return true;
			}
		}

		// The method returns false if this path leads to be a dead end
		return false;
	}


	private List<Node> GetAdjacentWalkableNodes(Node fromNode) {
		List<Node> walkableNodes = new List<Node>();
		IEnumerable<Vector2> nextLocations = GetAdjacentLocations(fromNode.location);

		foreach (var location in nextLocations){
			int x = (int)location.x;
			int y = (int)location.y;

			// Stay within the grid's boundaries
			if(x < 0 || x >= this.width || y < 0 || y >= this.height) {
				continue;
			}

			Node node = this.nodes[x, y];
			// Ignore non-walkable nodes
			if(!node.isWalkable) {
				continue;
			}
				
			// Ignore already-closed nodes
			if(node.State == NodeState.Closed) {
				continue;
			}


			// Already-open nodes are only added to the list if their G-value is lower going via this route.
			if (node.State == NodeState.Open) {
				float traversalCost = Node.GetTraversalCost(node.location, node.parentNode.location);
				float gTemp = fromNode.G + traversalCost;
				if (gTemp < node.G) {
					node.parentNode = fromNode;
					walkableNodes.Add(node);
				}
			} else {
				// If it's untested, set the parent and flag it as 'Open' for consideration
				node.parentNode = fromNode;
				node.State = NodeState.Open;
				walkableNodes.Add(node);
			}
		}

		return walkableNodes;
	}

	private static IEnumerable<Vector2> GetAdjacentLocations(Vector2 fromLocation) {
		return new Vector2[] {
			new Vector2(fromLocation.x-1, fromLocation.y-1),
			new Vector2(fromLocation.x-1, fromLocation.y  ),
			new Vector2(fromLocation.x-1, fromLocation.y+1),
			new Vector2(fromLocation.x,   fromLocation.y+1),
			new Vector2(fromLocation.x+1, fromLocation.y+1),
			new Vector2(fromLocation.x+1, fromLocation.y  ),
			new Vector2(fromLocation.x+1, fromLocation.y-1),
			new Vector2(fromLocation.x,   fromLocation.y-1)
		};
	}
}
