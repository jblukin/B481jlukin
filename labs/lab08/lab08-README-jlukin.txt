B481 / Fall 2023 -- Jonah Lukin (jlukin) -- Lab 08

Psuedocode

interpolate red (inversed to make (0,0) have red = 1) and green (standard linear interpolation)

use those interpolations values to calculate blue

output color



Question Answers

Unity uses Meshs and a list of Mesh vertices which store position (float3/4), normal (float3), the texture coordinates of all textures of the mesh,
the tangent (float4), and the color (float4).

Unity uses v2f struct to pass data from the vertex shader to the fragment shader, it can use any of the data types from the vertices.
v2f represents the modified vertex data after running through the vertex shader.