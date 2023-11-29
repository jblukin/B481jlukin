B481 / Fall 2023 -- Jonah Lukin (jlukin) -- HW 06

Part C

1) Lambert's law says the intensity percieved by the observer is based on the cosine of the angle between the viewing direction and the surface normal.
   The intensity is gets higher when the observation angle and the surface normal get closer together.
   
2)

a) The ambient color on a material is a simple way to approximate the low/no-light illumination of an object.

b) R = N cos(theta)

Theta is the angle between L and N

N is the normal of P

Vanishes when theta is 90 degrees, or cos(theta) > 1, or cos(theta) < -1

Part D

1) One normal per face

2) You would use the fragment shader and calculate the average normal of each vertex per fragment,
   and use that normal to calculate the illumination effect, instead of the face normal.
   
3) Light Direction World Vector, Camera/Eye Direction World Vector, Surface Object Mesh, 
   and Normal Directions of each face
   
Part E

1) R = 2(L (dot) N)N - L

Phong = (R (dot) C)^f

2) If the angle between L and N is greater than 90 degrees and when it is less than 0 degrees