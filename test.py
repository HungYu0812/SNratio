from PIL import Image
import numpy as np

im = Image.open("./test/ClearOutside.tif")
im.seek(0)

imarray = np.array(im)
temp = []
for i in imarray:
    for j in i:
        if j > 0:
            temp.append(j)
temp.sort()
print(temp[-80000:-2000])
