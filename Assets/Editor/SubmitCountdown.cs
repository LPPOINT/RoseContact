﻿using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Editor
{
    public class SubmitCountdown : EditorWindow
    {

        public static DateTime SubmitDate = new DateTime(2015, 5, 2, 20, 0, 0);

        [MenuItem("Tools/Submit Countdown")]
        public static void Initialize()
        {
            var w = GetWindow<SubmitCountdown>();
            w.title = "Обратынй отсчёт";
            w.NextQuote();
            w.Show();

        }

        private static TimeSpan quoteDuration = new TimeSpan(0, 0, 0, 15);
        private static string[] quotes = new string[]
                                   {
                                       "Делай сегодня то, что другие не хотят. Завтра будешь жить так, как другие не могут. © Джаред Лето",
                                       "Я не терпел поражений. Я просто нашёл 10 000 способов, которые не работают. © Томас Эдисон",
                                       "Если проблему можно разрешить, не стоит о ней беспокоиться. Если проблема неразрешима, беспокоиться о ней бессмысленно. © Далай Лама ",
                                       "Даже если вы очень талантливы и прилагаете большие усилия, для некоторых результатов просто требуется время: вы не получите ребенка через месяц, даже если заставите забеременеть девять женщин. © Уоррен Баффет ",
                                       "Раз в жизни фортуна стучится в дверь каждого человека, но человек в это время нередко сидит в ближайшей пивной и никакого стука не слышит. © Марк Твен ",
                                       "Наш большой недостаток в том, что мы слишком быстро опускаем руки. Наиболее верный путь к успеху – все время пробовать еще один раз. © Томас Эдисон ",
                                       "Если хотите иметь успех, Вы должны выглядеть так, как будто Вы его имеете. © Томас Мор ",
                                       "Лично я люблю землянику со сливками, но рыба почему-то предпочитает червяков. Вот почему, когда я иду на рыбалку, я думаю не о том, что люблю я, а о том, что любит рыба. © Дейл Карнеги",
                                       "Просыпаясь утром, спроси себя: «Что я должен сделать?» Вечером, прежде чем заснуть: «Что я сделал?». © Пифагор ",
                                       "Старики всегда советуют молодым экономить деньги. Это плохой совет. Не копите пятаки. Вкладывайте в себя. Я в жизни не сэкономил и доллара, пока не достиг сорока лет. © Генри Форд ",
                                       "Тяжёлый труд - это скопление легких дел, которые вы не сделали, когда должны были сделать. © Джон Максвелл ",
                                       "Я этого хочу. Значит, это будет. © Генри Форд ",
                                       "Урок, который я извлек и которому следую всю жизнь, состоял в том, что надо пытаться, и пытаться, и опять пытаться - но никогда не сдаваться! © Ричард Бренсон",
                                       "Ваш образ мыслей сделал вас тем, кем вы сегодня являетесь. Но он не приведет вас к той цели, которой вам хотелось бы достичь. © Бодо Шеффер ",
                                       " Заработайте себе репутацию, и она будет работать на вас. © Джон Дэвисон Рокфеллер ",
                                       "Оправдания — это ложь, которую вы говорите самим себе. Прекратите хныкать, жаловаться и вести себя как дети. Оправдания делают человека бедным. © Роберт Кийосаки ",
                                       "Единственный тормоз на пути к нашим завтрашним достижениям – это наши сегодняшние сомнения. © Франклин Рузвельт"
                                   };


        private int currentQuoteIndex;
        private DateTime currentQuoteStartTime;

        private void NextQuote()
        {
            currentQuoteIndex = Random.Range(0, quotes.Length);
            currentQuoteStartTime = DateTime.Now;
        }

        private void OnGUI()
        {
            if (SubmitDate < DateTime.Now)
            {
                EditorGUILayout.TextArea("ОБРАТНЫЙ ОТСЧЁТ ЗАВЕРШЁН!");
            }
            else
            {
                var cd = (SubmitDate - DateTime.Now);
                //EditorGUILayout.LabelField("ДО ОТПРАВКИ ОСТАЛОСЬ:");
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox(cd.Days + " Дней " + cd.Hours + " Часов " + cd.Minutes + " Минут " + cd.Seconds + " Секунд осталось до сдачи на проверку.", MessageType.Warning);
                EditorGUILayout.Space();
                GUI.skin.box.normal.textColor = Color.white;
                GUILayout.Box(quotes[currentQuoteIndex]);

            }

            if ((DateTime.Now - currentQuoteStartTime) > quoteDuration)
            {
                NextQuote();
            }
        }

        private void Update()
        {
            Repaint();
        }
    }
}