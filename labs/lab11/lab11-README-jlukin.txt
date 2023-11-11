B481 / Fall 2023 -- Jonah Lukin (jlukin) -- Lab 11

Could not figure out how to color the faces uniquely

All other parts completed

Question Answers:

1) How do you use a Mesh to prepare the vertices that will define a 3D cube model?

	You create a list of vertices and triangles to set for the mesh.

2) To render the 3D cube, Mesh vertices are processed by a Mesh Renderer, which passes each vertex to its own vertex shader instance. How does the vertex shader transform coordinates for each vertex, from object coordinates to clip coordinates?

	The shader use a pre-determined matrix to convert the coordinates/