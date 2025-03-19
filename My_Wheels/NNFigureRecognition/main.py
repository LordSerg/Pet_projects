import numpy as np
import pygame

import tensorflow as tf
#from tensorflow import keras
from tensorflow.keras.models import load_model
from tensorflow.keras.preprocessing.image import ImageDataGenerator
from tensorflow.keras.preprocessing import image
from tensorflow.keras.models import Sequential
from tensorflow.keras.layers import Conv2D, MaxPooling2D, Flatten, Dense
# Завантаження моделі з файлу

model_new = load_model('mySuperModel.h5')
'''
model_new = Sequential([
    Conv2D(32, kernel_size=(3, 3), activation='relu', input_shape=(70, 70, 1), padding='same'),
    MaxPooling2D(pool_size=(2, 2),padding='same'),
    Conv2D(64, kernel_size=(3, 3), activation='relu', padding='same'),
    MaxPooling2D(pool_size=(2, 2),padding='same'),
    Flatten(),
    Dense(512, activation='relu'),
    Dense(3, activation='softmax')
])

# Копіювання ваг моделі зі старої моделі у нову
for i in range(len(model_new.layers)):
    model_new.layers[i].set_weights(qwerty.layers[i].get_weights())

# Компіляція нової моделі
model_new.compile(optimizer='adam', loss='sparse_categorical_crossentropy', metrics=['accuracy'])

# Виведення структури нової моделі
'''
model_new.compile(optimizer='adam', loss='categorical_crossentropy', metrics=['accuracy'])
model_new.summary()


#model = create_model()
#model.load_weights('aboba.weights.h5')


def rgbToGrayScale(image):
    h,w,_=image.shape
    result = np.zeros((h,w,1))
    for i in range(h):
        for j in range(w):
            result[i,j,0]=np.mean(image[j,i])
    
    return result

pygame.init()
screen = pygame.display.set_mode((700,700))
pygame.display.set_caption("drawing game")
clock = pygame.time.Clock()
screen.fill((255,255,255)) # очищаємо екран
pygame.init()
loop = True
press = False
while loop:
    try:
        #pygame.init()
        #pygame.mouse.set_visible(False)
        #print(pygame.event.get())
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                loop = False
    
        px, py = pygame.mouse.get_pos()
        px=px/10
        py=py/10
        if pygame.mouse.get_pressed() == (1,0,0):
            pygame.draw.rect(screen, (0,0,0), (px,py,5,5))
            press = True

        if press == True and pygame.mouse.get_pressed() == (0,0,0): #or event.type == pygame.MOUSEBUTTONUP
            press = False
            
            # отримуємо зображення 70*70
            rect = pygame.Rect(0, 0, 70, 70)
            sub = screen.subsurface(rect)            
            
            # конвертуємо у масив масивів масивів: [[[r,g,b],...,[r,g,b]]
            #                                       [[r,g,b],...,[r,g,b]]
            #                                       ...
            #                                       [[r,g,b],...,[r,g,b]]
            #                                       [[r,g,b],...,[r,g,b]]]
            string_image = pygame.image.tostring(sub, 'RGB')
            temp_surf = pygame.image.fromstring(string_image,(70,70),'RGB')
            tmp_arr = np.array(pygame.surfarray.array3d(temp_surf))
            # оскільки ми знаємо, що в даному квадратику всі пікселі або чорні(0,0,0), або білі(255,255,255)
            # то робимо з нього масив масивів:     [[c,c,...,c]
            #                                       [c,c,...,c]
            #                                       ...
            #                                       [c,c,...,c]]
            arr = rgbToGrayScale(tmp_arr)
            arr = np.expand_dims(arr, axis=0)
            drawing = ImageDataGenerator(rescale=1./255)
            drawing.fit(arr)
            
            prediction = model_new.predict(drawing.flow(arr))
            predicted_class = np.argmax(prediction)
            
            if predicted_class == 0:
                print("Ellipse")
            elif predicted_class == 1:
                print("Rectangle")
            else:
                print("Triangle")
            
            screen.fill((255,255,255))# очищаємо екран
        pygame.display.update()
        clock.tick(1000)
    except Exception as e:
        print(e)
        pygame.quit()
        break
        
pygame.quit()


#reorganise data to that view:
'''
/train
|----> circle
|----> rectangle
|----> triangle
/test
|----> circle
|----> rectangle
|----> triangle
'''

'''
#реорганізація теки
import os
import shutil

# Шлях до папки з даними
data_dir = 'data'

# Створення списку типів зображень
image_types = ['ellipse', 'rectangle', 'triangle']

# Перебір усіх папок користувачів
for user_folder in os.listdir(data_dir):
    user_folder_path = os.path.join(data_dir, user_folder)
    
    # Перебір усіх типів зображень
    for image_type in image_types:
        type_folder_path = os.path.join(user_folder_path, 'images', image_type)
        
        # Перевірка наявності папки з поточним типом зображень
        if os.path.exists(type_folder_path):
            # Створення папки в кореневій директорії
            new_type_folder_path = os.path.join(data_dir, image_type)
            if not os.path.exists(new_type_folder_path):
                os.makedirs(new_type_folder_path)
            
            # Перенесення зображень до нової папки
            for image_file in os.listdir(type_folder_path):
                old_image_path = os.path.join(type_folder_path, image_file)
                new_image_path = os.path.join(new_type_folder_path, image_file)
                shutil.move(old_image_path, new_image_path)
            
            # Видалення порожньої папки з типом зображень
            os.rmdir(type_folder_path)

print("Операція завершена!")
'''

'''
#зміна назв зображень
#e1.png, де е - означає elipse
import os

# Шлях до папки з зображеннями
folder_path = 'data/triangle'

# Отримання списку зображень у папці
images = os.listdir(folder_path)

# Загальна кількість зображень
total_images = len(images)

# Перебір усіх зображень та їх переіменування
for i, image_name in enumerate(images):
    # Оригінальний шлях до зображення
    original_image_path = os.path.join(folder_path, image_name)
    
    # Нове ім'я зображення з пронумерованою послідовністю
    new_image_name = f"t{i}.{image_name.split('.')[-1]}"  # Формуємо ім'я типу 0.jpg, 1.jpg і так далі
    
    # Новий шлях до зображення
    new_image_path = os.path.join(folder_path, new_image_name)
    
    # Перейменування зображення
    os.rename(original_image_path, new_image_path)

print("Зображення успішно переіменовано!")
'''
'''
#розділяємо дані на тренувальні та перевірочні
import os
import random
import shutil

# Шлях до папки з даними
data_dir = 'data'

# Папки для навчального та тестувального наборів
train_dir = os.path.join(data_dir, 'train')
test_dir = os.path.join(data_dir, 'test')

# Створення папок train та test, якщо вони не існують
if not os.path.exists(train_dir):
    os.makedirs(train_dir)
if not os.path.exists(test_dir):
    os.makedirs(test_dir)

# Відсоткове співвідношення для навчального та тестувального наборів (80% - навчальний, 20% - тестувальний)
train_percentage = 0.8

# Створення списку категорій
categories = ['ellipse', 'rectangle', 'triangle']

# Перебір усіх категорій
for category in categories:
    # Шлях до папок з категоріями в оригінальній структурі
    original_category_dir = os.path.join(data_dir, category)
    
    # Шлях до папок з категоріями в новій структурі
    train_category_dir = os.path.join(train_dir, category)
    test_category_dir = os.path.join(test_dir, category)
    
    # Створення папок для категорій в новій структурі
    if not os.path.exists(train_category_dir):
        os.makedirs(train_category_dir)
    if not os.path.exists(test_category_dir):
        os.makedirs(test_category_dir)
    
    # Отримання списку зображень у поточній категорії
    images = os.listdir(original_category_dir)
    
    # Випадкове перемішування списку зображень
    random.shuffle(images)
    
    # Обчислення кількості зображень для навчального та тестувального наборів
    num_train_images = int(len(images) * train_percentage)
    num_test_images = len(images) - num_train_images
    
    # Переміщення зображень до відповідних папок
    for i, image_name in enumerate(images):
        original_image_path = os.path.join(original_category_dir, image_name)
        if i < num_train_images:
            train_image_path = os.path.join(train_category_dir, image_name)
            shutil.copyfile(original_image_path, train_image_path)
        else:
            test_image_path = os.path.join(test_category_dir, image_name)
            shutil.copyfile(original_image_path, test_image_path)

print("Операція завершена!")
'''