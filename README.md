# 🚨 AR-Secours — Réalité Augmentée pour les Premiers Secours

Application Android de réalité augmentée pour apprendre les gestes de premiers secours,
développée avec Unity 2022.3 + AR Foundation.

**ENSIM Le Mans Université — 2025-2026**
**Auteur :** Hassen Adam
**Encadrant :** Corentin Coupry

---

## 📅 Journal de développement

---

### Séance 1 – TP Réalité Augmentée

Dans cette première séance de TP, j'ai commencé par créer un objet simple (un cube) dans Unity pour tester le fonctionnement de la réalité augmentée dans mon projet. Ce cube sert d'objet de test avant d'intégrer plus tard un vrai modèle lié aux gestes de premiers secours.

Ensuite, j'ai transformé ce cube en **Prefab** afin de pouvoir l'utiliser facilement et le faire apparaître dans la scène quand l'utilisateur interagit avec l'application.

Après cela, j'ai créé un script en C# qui permet de placer cet objet dans l'environnement lorsque l'utilisateur touche l'écran du téléphone. Le script détecte une surface et instancie le cube à cet endroit.

Cette étape m'a permis de comprendre comment faire apparaître un objet virtuel dans l'environnement réel, ce qui sera utilisé plus tard pour afficher des éléments pédagogiques liés aux premiers secours.

---

### Séance 2 – TP Réalité Augmentée

Dans cette deuxième séance, j'ai d'abord amélioré le script `PlaceObject` en C# pour qu'il ne crée plus un nouvel objet à chaque toucher, mais **déplace le même objet** sur la surface détectée. Cela permet une interaction plus propre et cohérente avec l'environnement réel.

Ensuite, j'ai créé un système d'étapes pédagogiques via un nouveau script `FirstAidSteps`. Ce script gère un tableau d'étapes (titre + instruction) et permet de naviguer entre elles. Les étapes sont affichées dynamiquement dans l'interface utilisateur.

J'ai également construit une **interface UI complète** avec un Canvas en Screen Space Overlay contenant un panel d'instructions, trois textes (titre, instruction, compteur d'étapes) et deux boutons **Suivant / Précédent** connectés aux fonctions du script via les événements OnClick.

Les trois étapes de premiers secours suivantes ont été intégrées : vérification de la sécurité de la zone, vérification de la conscience de la victime, et appel des secours (15 ou 112).

Enfin, j'ai compilé et déployé l'application sur un **Google Pixel 9 Pro XL** via Android Build, et vérifié le bon fonctionnement de la caméra AR, de la détection de surface et de l'affichage de l'interface utilisateur sur le téléphone.

---

### Séance 3 – TP Réalité Augmentée

Dans cette troisième et dernière séance, j'ai apporté les évolutions les plus importantes du projet.

**Intégration des personnages 3D animés :**
J'ai téléchargé deux modèles humains animés depuis **Mixamo (Adobe)** au format FBX for Unity :
- `Receiving CPR` — la victime allongée
- `Administering CPR` — le secouriste effectuant le massage cardiaque

Ces deux personnages sont instanciés simultanément dans la scène AR lorsque l'utilisateur touche le sol. Un **Animator Controller** avec animation en boucle a été configuré pour que le secouriste joue le geste de massage cardiaque en continu.

**Refonte complète de l'interface utilisateur :**
L'interface a été entièrement redessinée depuis le code C# :
- Menu principal avec fond sombre, titre rouge, et boutons colorés par étape.
- Page de détail avec instructions claires et bouton **VOIR LA DÉMONSTRATION AR**
- Lors de l'activation de la démonstration, le fond devient transparent pour laisser voir la caméra et les personnages 3D dans l'environnement réel
- Bouton **RETOUR AU MENU** bien visible en bas de l'écran

**Corrections et améliorations :**
- Correction du bug de closure C# qui causait l'ouverture de la même étape quel que soit le bouton cliqué
- Amélioration du positionnement relatif des deux personnages dans la scène AR

---

