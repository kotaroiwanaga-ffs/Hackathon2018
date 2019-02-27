##
# cording:utf-8
##

import json
import numpy as np
import cv2
from pathlib import Path
import sys


def analyze_hand_json(json_path):
    with open(json_path, 'r') as f:
        json_data = json.load(f)

    dict_people = []
    hands = {'hand_left_keypoints_2d': 'left_hand', 'hand_right_keypoints_2d': 'right_hand'}
    fingers = {3: 'thumb',
               7: 'index finger',
               11: 'middle finger',
               15: 'ring finger',
               19: 'little finger'}

    for person in json_data['people']:
        dict_person = {}

        for hand in hands:
            if hand in person:
                dict_person[hands[hand]] = {}

                for finger in fingers:
                    first_joint = [int(person[hand][finger * 3]), int(person[hand][(finger * 3) + 1])]
                    second_joint = [int(person[hand][(finger + 1) * 3]), int(person[hand][((finger + 1) * 3) + 1])]

                    dict_person[hands[hand]][fingers[finger]] = [first_joint, second_joint]
        dict_people.append(dict_person)

    print(dict_people)
    return dict_people


def draw_coordinates(people, image_path, output_path):
    image = cv2.imread(image_path)

    for pt1, pt2 in get_coordinates_itr(people):
        image = cv2.line(img=image, pt1=pt1, pt2=pt2, color=(255, 0, 0), thickness=10)

    print(image)
    cv2.imwrite(output_path, image)


def draw_coordinates_mosaic(people, image_path, output_path):
    image = cv2.imread(image_path)

    for pt1, pt2 in get_coordinates_itr(people):
        x1 = min(pt1[0], pt2[0])
        x2 = max(pt1[0], pt2[0])
        y1 = min(pt1[1], pt2[1])
        y2 = max(pt1[1], pt2[1])

        image = cv2.rectangle(img=image, pt1=pt1, pt2=pt2, color=(0, 0, 255), thickness=2)
        clipped = image[y1:y2, x1:x2]
        clipped = cv2.blur(clipped, (10, 10))
        image[y1:y2, x1:x2] = clipped

    cv2.imwrite(output_path, image)


def get_coordinates_itr(people):
    for person in people:
        for hand_key in person:
            hand = person[hand_key]
            for finger in hand:
                pt1 = (hand[finger][0][0], hand[finger][0][1])
                pt2 = (hand[finger][1][0], hand[finger][1][1])

                yield pt1, pt2


def main(json_path, image_path, output_path):
    people = analyze_hand_json(json_path)
    draw_coordinates_mosaic(people, image_path, output_path)


if __name__ == '__main__':
    args = sys.argv
    if len(args) >= 4:
        main(args[1], args[2], args[3])
    else:
        main()
