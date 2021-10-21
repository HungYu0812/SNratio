from PIL import Image
import numpy as np
import os
import matplotlib.pyplot as plt
import math
import sys


def dataset(filename, step=1):
    f = open(filename, 'r')
    depth = 0
    sData = []
    bData = []
    dData = []
    for line in f:
        sData.append(float(line.split()[0]))
        bData.append(float(line.split()[1]))
        dData.append(depth)
        depth = depth + step
    return np.array(sData), np.array(bData), np.array(dData)


'''
def draw(x1, x2, y1, y2):
    def press(event):
        print('press', event.key)
        sys.stdout.flush()
        if event.key == 'z':
            number = -1
        if event.key == 'x':
            number = +1
        for i in range(len(y2)):
            x2[i] = x2[i] + number
        ax.clear()
        line1 = ax.plot(x1, y1, color="red", label="My Line 1")
        line2 = ax.plot(x2, y2, color="blue", label="My Line 2")
        plt.show()
    fig, ax = plt.subplots(1, figsize=(8, 6))
    fig.suptitle('Title Title', fontsize=15)
    fig.canvas.mpl_connect('key_press_event', press)
    plt.xlabel('Depth(um)')
    plt.ylabel('Signal/Background')
    line1 = ax.plot(x1, y1, color="red", label="My Line 1")
    line2 = ax.plot(x2, y2, color="blue", label="My Line 2")
    plt.show()

'''
#dirName = input("Enter the directory name: ")

#fileName1x = dirName + "\\1xPBS.txt"
#fileName2x = dirName + "\\0.5xPBS.txt"

#g1, g2, h1 = dataset("test_1_2.txt")

#fileName = "test_1.txt"
#fileName = "test_10.txt"
#x1, x2, y1 = dataset(fileName)


def press(event):
    print('press', event.key)
    global move
    print('Move', move)
    sys.stdout.flush()

    number = 0
    if event.key == 'z':
        number = -1
    if event.key == 'x':
        number = +1
    for i in range(len(x1)):
        y1[i] = y1[i] + number
    move = move+number
    ax.clear()
    line1 = ax.plot(y1, x1/x2, color="red", label="0_1")

    line2 = ax.plot(a3, a1/a2, color="blue", label="0_2")
    line3 = ax.plot(b3, b1/b2, color="yellow", label="0_1_o12")
    line5 = ax.plot(c3, c1/c2, color="green", label="1_2_o12")
    ax.legend(0)
    plt.xlabel('Depth(um)')
    plt.ylabel('Signal/Background')
    plt.show()


dirPath = input("Enter the directory path: ")
x1, x2, y1 = dataset(dirPath+"test_0_1.txt")
a1, a2, a3 = dataset(dirPath+"test_0_2.txt")
b1, b2, b3 = dataset(dirPath+"test_0_1_o12.txt")
c1, c2, c3 = dataset(dirPath+"test_1_2_o12.txt")

move = 0
fig, ax = plt.subplots(1, figsize=(8, 6))
fig.suptitle('SBR for different signal section', fontsize=15)
fig.canvas.mpl_connect('key_press_event', press)
plt.xlabel('Depth(um)')
plt.ylabel('Signal/Background')

line1 = ax.plot(y1, x1/x2, color="red", label="0_1")
line2 = ax.plot(a3, a1/a2, color="blue", label="0_2")
line3 = ax.plot(b3, b1/b2, color="yellow", label="0_1_o12")
line5 = ax.plot(c3, c1/c2, color="green", label="1_2_o12")
#line4 = ax.plot(d3, d1/d2, color="purple", label="1_3")
#line6 = ax.plot(e3, e1/e2, color="black", label="2_3")
plt.legend()
plt.show()
