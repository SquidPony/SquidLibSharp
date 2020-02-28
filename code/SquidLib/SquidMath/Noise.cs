﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SquidLib.SquidMath
{
    public interface INoise2D
    {
        double GetNoise(double x, double y);

        double GetNoiseSeeded(double x, double y, long seed);
    }
    public interface INoise3D
    {
        double GetNoise(double x, double y, double z);

        double GetNoiseSeeded(double x, double y, double z, long seed);
    }

    /*
 * Derived from Joise, which is derived from the Accidental Noise Library.
 * Licenses for these projects are as follows
 * 
 * ============================================================================
 * | Joise
 * ============================================================================
 * 
 * Copyright (C) 2016 Jason Taylor
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * ============================================================================
 * | Accidental Noise Library
 * | --------------------------------------------------------------------------
 * | Joise is a derivative work based on Josua Tippetts' C++ library:
 * | http://accidentalnoise.sourceforge.net/index.html
 * ============================================================================
 * 
 * Copyright (C) 2011 Joshua Tippetts
 * 
 *   This software is provided 'as-is', without any express or implied
 *   warranty.  In no event will the authors be held liable for any damages
 *   arising from the use of this software.
 * 
 *   Permission is granted to anyone to use this software for any purpose,
 *   including commercial applications, and to alter it and redistribute it
 *   freely, subject to the following restrictions:
 * 
 *   1. The origin of this software must not be misrepresented; you must not
 *      claim that you wrote the original software. If you use this software
 *      in a product, an acknowledgment in the product documentation would be
 *      appreciated but is not required.
 *   2. Altered source versions must be plainly marked as such, and must not be
 *      misrepresented as being the original software.
 *   3. This notice may not be removed or altered from any source distribution.
 */
    public class SimplexNoise : INoise2D, INoise3D
    {
        #region VECTORS
        internal static readonly double[] PhiGrad2 = {
            0.6499429579167653, 0.759982994187637,
            -0.1551483029088119, 0.9878911904175052,
            -0.8516180517334043, 0.5241628506120981,
            -0.9518580082090311, -0.30653928330368374,
            -0.38568876701087174, -0.9226289476282616,
            0.4505066120763985, -0.8927730912586049,
            0.9712959670388622, -0.23787421973396244,
            0.8120673355833279, 0.5835637432865366,
            0.08429892519436613, 0.9964405106232257,
            -0.702488350003267, 0.7116952424385647,
            -0.9974536374007479, -0.07131788861160528,
            -0.5940875849508908, -0.804400361391775,
            0.2252075529515288, -0.9743108118529653,
            0.8868317111719171, -0.4620925405802277,
            0.9275724981153959, 0.373643226540993,
            0.3189067150428103, 0.9477861083074618,
            -0.5130301507665112, 0.8583705868705491,
            -0.9857873824221494, 0.1679977281313266,
            -0.7683809836504446, -0.6399927061806058,
            -0.013020236219374872, -0.9999152331316848,
            0.7514561619680513, -0.6597830223946701,
            0.9898275175279653, 0.14227257481477412,
            0.5352066871710182, 0.8447211386057674,
            -0.29411988281443646, 0.9557685360657266,
            -0.9175289804081126, 0.39766892022290273,
            -0.8985631161871687, -0.43884430750324743,
            -0.2505005588110731, -0.968116454790094,
            0.5729409678802212, -0.8195966369650838,
            0.9952584535626074, -0.09726567026534665,
            0.7207814785200723, 0.6931623620930514,
            -0.05832476124070039, 0.998297662136006,
            -0.7965970142012075, 0.6045107087270838,
            -0.977160478114496, -0.21250270589112422,
            -0.4736001288089817, -0.8807399831914728,
            0.36153434093875386, -0.9323587937709286,
            0.9435535266854258, -0.3312200813348966,
            0.8649775992346886, 0.5018104750024599,
            0.1808186720712497, 0.9835164502083277,
            -0.6299339540895539, 0.7766487066139361,
            -0.9996609468975833, 0.02603826506945166,
            -0.6695112313914258, -0.7428019325774111,
            0.12937272671950842, -0.9915960354807594,
            0.8376810167470904, -0.5461597881403947,
            0.959517028911149, 0.28165061908243916,
            0.4095816551369482, 0.9122734610714476,
            -0.42710760401484793, 0.9042008043530463,
            -0.9647728141412515, 0.2630844295924223,
            -0.8269869890664444, -0.562221059650754,
            -0.11021592552380209, -0.9939076666174438,
            0.6837188597775012, -0.72974551782423,
            0.998972441738333, 0.04532174585508431,
            0.6148313475439905, 0.7886586169422362,
            -0.1997618324529528, 0.9798444827088829,
            -0.8744989400706802, 0.48502742583822706,
            -0.9369870231562731, -0.3493641630687752,
            -0.3434772946489506, -0.9391609809082988,
            0.4905057254335028, -0.8714379687143274,
            0.9810787787756657, -0.1936089611460388,
            0.7847847614201463, 0.6197684069414349,
            0.03905187955516296, 0.9992371844077906,
            -0.7340217731995672, 0.6791259356474049,
            -0.9931964444524306, -0.1164509455824639,
            -0.5570202966000876, -0.830498879695542,
            0.2691336060685578, -0.9631028512493016,
            0.9068632806061, -0.4214249521425399,
            0.9096851999779008, 0.4152984913783901,
            0.27562369868737335, 0.9612656119522284,
            -0.5514058359842319, 0.8342371389734039,
            -0.9923883787916933, 0.12314749546456379,
            -0.7385858406439617, -0.6741594440488484,
            0.032311046904542805, -0.9994778618098213,
            0.7805865154410089, -0.6250477517051506,
            0.9823623706068018, 0.18698709264487903,
            0.49637249435561115, 0.8681096398768929,
            -0.3371347561867868, 0.9414564016304079,
            -0.9346092156607797, 0.35567627697379833,
            -0.877750600058892, -0.47911781859606817,
            -0.20636642697019966, -0.9784747813917093,
            0.6094977881394418, -0.7927877687333024,
            0.998644017504346, -0.052058873429796634,
            0.6886255051458764, 0.7251171723677399,
            -0.10350942208147358, 0.9946284731196666,
            -0.8231759450656516, 0.567786371327519,
            -0.9665253951623188, -0.2565709658288005,
            -0.43319680340129196, -0.9012993562201753,
            0.4034189716368784, -0.9150153732716426,
            0.9575954428121146, -0.28811624026678895,
            0.8413458575409575, 0.5404971304259356,
            0.13605818775026976, 0.9907008476558967,
            -0.664485735550556, 0.7473009482463117,
            -0.999813836664718, -0.01929487014147803,
            -0.6351581891853917, -0.7723820781910558,
            0.17418065221630152, -0.984713714941304,
            0.8615731658120597, -0.5076334109892543,
            0.945766171482902, 0.32484819358982736,
            0.3678149601703667, 0.9298990026206456,
            -0.4676486851245607, 0.883914423064399,
            -0.9757048995218635, 0.2190889067228882,
            -0.8006563717736747, -0.5991238388999518,
            -0.06505704156910719, -0.9978815467490495,
            0.716089639712196, -0.6980083293893113,
            0.9958918787052943, 0.09055035024139549,
            0.5784561871098056, 0.8157134543418942,
            -0.24396482815448167, 0.9697840804135497,
            -0.8955826311865743, 0.4448952131872543,
            -0.9201904205900768, -0.39147105876968413,
            -0.3005599364234082, -0.9537629289384008,
            0.5294967923694863, -0.84831193960148,
            0.9888453593035162, -0.1489458135829932,
            0.7558893631265085, 0.6546993743025888,
            -0.006275422246980369, 0.9999803093439501,
            -0.764046696121276, 0.6451609459244744,
            -0.9868981170802014, -0.16134468229090512,
            -0.5188082666339063, -0.8548906260290385,
            0.31250655826478446, -0.9499156020623616,
            0.9250311403279032, -0.3798912863223621,
            0.889928392754896, 0.45610026942404636,
            0.2317742435145519, 0.9727696027545563,
            -0.5886483179573486, 0.8083892365475831,
            -0.996949901406418, 0.0780441803450664,
            -0.707272817672466, -0.7069407057042696,
            0.07757592706207364, -0.9969864470194466,
            0.8081126726681943, -0.5890279350532263,
            0.9728783545459001, 0.23131733021125322,
            0.4565181982253288, 0.8897140746830408,
            -0.3794567783511009, 0.9252094645881026,
            -0.9497687200714887, 0.31295267753091066,
            -0.8551342041690687, -0.5184066867432686,
            -0.16180818807538452, -0.9868222283024238,
            0.6448020194233159, -0.7643496292585048,
            0.9999772516247822, -0.006745089543285545,
            0.6550543261176665, 0.7555817823601425,
            -0.14848135899860646, 0.9889152066936411,
            -0.848063153443784, 0.5298951667745091,
            -0.9539039899003245, -0.300111942535184,
            -0.3919032080850608, -0.9200064540494471,
            0.44447452934057863, -0.8957914895596358,
            0.9696693887216105, -0.24442028675267172,
            0.8159850520735595, 0.5780730012658526,
            0.0910180879994953, 0.9958492394217692,
            -0.6976719213969089, 0.7164173993520435,
            -0.9979119924958648, -0.06458835214597858,
            -0.5994998228898376, -0.8003748886334786,
            0.2186306161766729, -0.9758076929755208,
            0.8836946816279001, -0.46806378802740584,
            0.9300716543684309, 0.36737816720699407,
            0.32529236260160294, 0.9456134933645286,
            -0.5072286936943775, 0.8618114946396893,
            -0.9846317976415725, 0.17464313062106204,
            -0.7726803123417516, -0.6347953488483143,
            -0.019764457813331488, -0.9998046640256011,
            0.7469887719961158, -0.6648366525032559,
            0.9907646418168752, 0.13559286310672486,
            0.5408922318074902, 0.8410919055432124,
            -0.2876664477065717, 0.9577306588304888,
            -0.9148257956391065, 0.40384868903250853,
            -0.9015027194859215, -0.4327734358292892,
            -0.2570248925062563, -0.9664047830139022,
            0.5673996816983953, -0.8234425306046317,
            0.9945797473944409, -0.10397656501736473,
            0.7254405241129018, 0.6882848581617921,
            -0.05158982732517303, 0.9986683582233687,
            -0.7925014140531963, 0.609870075281354,
            -0.9785715990807187, -0.20590683687679034,
            -0.47953002522651733, -0.8775254725113429,
            0.35523727306945746, -0.9347761656258549,
            0.9412979532686209, -0.33757689964259285,
            0.868342678987353, 0.4959647082697184,
            0.18744846526420056, 0.9822744386728669,
            -0.6246810590458048, 0.7808800000444446,
            -0.9994625758058275, 0.03278047534097766,
            -0.674506266646887, -0.738269121834361,
            0.12268137965007223, -0.9924461089082646,
            0.8339780641890598, -0.5517975973592748,
            0.9613949601033843, 0.2751721837101493,
            0.41572570400265835, 0.9094900433932711,
            -0.42099897262033487, 0.907061114287578,
            -0.9629763390922247, 0.2695859238694348,
            -0.8307604078465821, -0.5566301687427484,
            -0.11691741449967302, -0.9931416405461567,
            0.6787811074228051, -0.7343406622310046,
            0.999255415972447, 0.03858255628819732,
            0.6201369341201711, 0.7844935837468874,
            -0.19314814942146824, 0.9811696042861612,
            -0.8712074932224428, 0.4909149659086258,
            -0.9393222007870077, -0.34303615422962713,
            -0.3498042060103595, -0.9368228314134226,
            0.4846166400948296, -0.8747266499559725,
            0.9797505510481769, -0.20022202106859724,
            0.7889473022428521, 0.6144608647291752,
            0.045790935472179155, 0.9989510449609544,
            -0.7294243101497431, 0.684061529222753,
            -0.9939593229024027, -0.10974909756074072,
            -0.562609414602539, -0.8267228354174018,
            0.26263126874523307, -0.9648962724963078,
            0.9040001019019392, -0.4275322394408211,
            0.9124657316291773, 0.4091531358824348,
            0.28210125132356934, 0.9593846381935018,
            -0.5457662881946498, 0.8379374431723614,
            -0.9915351626845509, 0.12983844253579577,
            -0.7431163048326799, -0.6691622803863227,
            0.02556874420628532, -0.9996730662170076,
            0.7763527553119807, -0.6302986588273021,
            0.9836012681423212, 0.1803567168386515,
            0.5022166799422209, 0.8647418148718223,
            -0.330776879188771, 0.9437089891455613,
            -0.9321888864830543, 0.3619722087639923,
            -0.8809623252471085, -0.47318641305008735,
            -0.21296163248563432, -0.9770605626515961,
            0.604136498566135, -0.7968808512571063,
            0.9982701582127194, -0.05879363249495786,
            0.6935008202914851, 0.7204558364362367,
            -0.09679820929680796, 0.9953040272584711,
            -0.8193274492343137, 0.5733258505694586,
            -0.9682340024187017, -0.25004582891994304,
            -0.4392662937408502, -0.8983569018954422,
            0.39723793388455464, -0.9177156552457467,
            0.9556302892322005, -0.2945687530984589,
            0.8449724198323217, 0.5348098818484104,
            0.14273745857559722, 0.9897605861618151,
            -0.6594300077680133, 0.7517659641504648,
            -0.9999212381512442, -0.01255059735959867,
            -0.6403535266476091, -0.768080308893523,
            0.16753470770767478, -0.9858661784001437,
            0.8581295336101056, -0.5134332513054668,
            0.9479357869928937, 0.31846152630759517,
            0.37407884501651706, 0.9273969040875156,
            -0.461675964944643, 0.8870486477034012,
            -0.9742049295269273, 0.22566513972130173,
            -0.8046793020829978, -0.5937097108850584,
            -0.07178636201352963, -0.9974200309943962,
            0.7113652211526822, -0.7028225395748172,
            0.9964799940037152, 0.08383091047075403,
            0.5839450884626246, 0.8117931594072332,
            -0.23741799789097484, 0.9714075840127259,
            -0.8925614000865144, 0.45092587758477687,
            -0.9228099950981292, -0.38525538665538556,
            -0.30698631553196837, -0.95171392869712,
            0.5237628071845146, -0.8518641451605984,
            0.9878182118285335, -0.15561227580071732,
            0.7602881737752754, 0.6495859395164404,
            4.6967723669845613E-4, 0.9999998897016406,
            -0.7596776469502666, 0.6502998329417794,
            -0.9879639510809196, -0.15468429579171308,
            -0.5245627784110601, -0.8513717704420726,
            0.3060921834538644, -0.9520018777441807,
            0.9224476966294768, -0.3861220622846781,
            0.8929845854878761, 0.45008724718774934,
            0.23833038910266038, 0.9711841358002995,
            -0.5831822693781987, 0.8123413326200348,
            -0.9964008074312266, 0.0847669213219385,
            -0.712025106726807, -0.7021540054650968,
            0.07084939947717452, -0.9974870237721009,
            0.8041212432524677, -0.5944653279629567,
            0.9744164792492415, 0.22474991650168097,
            0.462509014279733, 0.8866145790082576,
    };
        internal static readonly double[] Grad3d =
        {
                    -0.448549002408981,  1.174316525459290,  0.000000000000001,
                     0.000000000000001,  1.069324374198914,  0.660878777503967,
                     0.448549002408981,  1.174316525459290,  0.000000000000001,
                     0.000000000000001,  1.069324374198914, -0.660878777503967,
                    -0.725767493247986,  0.725767493247986, -0.725767493247986,
                    -1.069324374198914,  0.660878777503967,  0.000000000000001,
                    -0.725767493247986,  0.725767493247986,  0.725767493247986,
                     0.725767493247986,  0.725767493247986,  0.725767493247986,
                     1.069324374198914,  0.660878777503967,  0.000000000000000,
                     0.725767493247986,  0.725767493247986, -0.725767493247986,
                    -0.660878777503967,  0.000000000000003, -1.069324374198914,
                    -1.174316525459290,  0.000000000000003, -0.448549002408981,
                     0.000000000000000,  0.448549002408981, -1.174316525459290,
                    -0.660878777503967,  0.000000000000001,  1.069324374198914,
                     0.000000000000001,  0.448549002408981,  1.174316525459290,
                    -1.174316525459290,  0.000000000000001,  0.448549002408981,
                     0.660878777503967,  0.000000000000001,  1.069324374198914,
                     1.174316525459290,  0.000000000000001,  0.448549002408981,
                     0.660878777503967,  0.000000000000001, -1.069324374198914,
                     1.174316525459290,  0.000000000000001, -0.448549002408981,
                    -0.725767493247986, -0.725767493247986, -0.725767493247986,
                    -1.069324374198914, -0.660878777503967, -0.000000000000001,
                    -0.000000000000001, -0.448549002408981, -1.174316525459290,
                    -0.000000000000001, -0.448549002408981,  1.174316525459290,
                    -0.725767493247986, -0.725767493247986,  0.725767493247986,
                     0.725767493247986, -0.725767493247986,  0.725767493247986,
                     1.069324374198914, -0.660878777503967,  0.000000000000001,
                     0.725767493247986, -0.725767493247986, -0.725767493247986,
                    -0.000000000000004, -1.069324374198914, -0.660878777503967,
                    -0.448549002408981, -1.174316525459290, -0.000000000000003,
                    -0.000000000000003, -1.069324374198914,  0.660878777503967,
                     0.448549002408981, -1.174316525459290,  0.000000000000003,
            };
        #endregion
        internal const double
            F2 = 0.36602540378443864676372317075294,
            G2 = 0.21132486540518711774542560974902,
            F3 = 1.0 / 3.0,
            G3 = 0.5 / 3.0;
        private static double gradCoord3D(long seed, int x, int y, int z, double xd, double yd, double zd)
        {
            uint hash = CoreMath.Hash32(x, y, z, seed) * 3;
            return xd * Grad3d[hash] + yd * Grad3d[hash + 1] + zd * Grad3d[hash + 2];
        }

        public long Seed { get; set; }

        public SimplexNoise(long seed)
        {
            Seed = seed;
        }

        public SimplexNoise() : this(0x1337BEEF) { }

        public double GetNoise(double x, double y)
        {
            return GetNoiseSeeded(x, y, Seed);
        }

        public double GetNoiseSeeded(double x, double y, long seed)
        {
            double s = (x + y) * F2;
            int i = CoreMath.FastFloor(x + s),
                    j = CoreMath.FastFloor(y + s);
            double t = (i + j) * G2,
                    X0 = i - t,
                    Y0 = j - t,
                    x0 = x - X0,
                    y0 = y - Y0;
            int i1, j1;
            if (x0 > y0)
            {
                i1 = 1;
                j1 = 0;
            }
            else
            {
                i1 = 0;
                j1 = 1;
            }
            double
                    x1 = x0 - i1 + G2,
                    y1 = y0 - j1 + G2,
                    x2 = x0 - 1 + 2 * G2,
                    y2 = y0 - 1 + 2 * G2;
            double n = 0.0;
            uint
                    gi0 = CoreMath.Hash256(i, j, seed) << 1,
                    gi1 = CoreMath.Hash256(i + i1, j + j1, seed) << 1,
                    gi2 = CoreMath.Hash256(i + 1, j + 1, seed) << 1;
            // Calculate the contribution from the three corners
            double t0 = 0.75 - x0 * x0 - y0 * y0;
            if (t0 > 0)
            {
                t0 *= t0;
                n += t0 * t0 * (PhiGrad2[gi0] * x0 + PhiGrad2[gi0 + 1] * y0);
                // for 2D gradient
            }
            double t1 = 0.75 - x1 * x1 - y1 * y1;
            if (t1 > 0)
            {
                t1 *= t1;
                n += t1 * t1 * (PhiGrad2[gi1] * x1 + PhiGrad2[gi1 + 1] * y1);
            }
            double t2 = 0.75 - x2 * x2 - y2 * y2;
            if (t2 > 0)
            {
                t2 *= t2;
                n += t2 * t2 * (PhiGrad2[gi2] * x2 + PhiGrad2[gi2 + 1] * y2);
            }
            // Add contributions from each corner to get the noise value.
            // The result is scaled to return values in the interval [-1,1].
            return 9.125 * n;
        }


        public double GetNoise(double x, double y, double z)
        {
            return GetNoiseSeeded(x, y, z, Seed);
        }

        public double GetNoiseSeeded(double x, double y, double z, long seed)
        {
            double n = 0.0;
            double s = (x + y + z) * F3;
            int i = CoreMath.FastFloor(x + s),
                    j = CoreMath.FastFloor(y + s),
                    k = CoreMath.FastFloor(z + s);

            double t = (i + j + k) * G3;
            double X0 = i - t, Y0 = j - t, Z0 = k - t,
                    x0 = x - X0, y0 = y - Y0, z0 = z - Z0;

            int i1, j1, k1;
            int i2, j2, k2;

            if (x0 >= y0)
            {
                if (y0 >= z0)
                {
                    i1 = 1;
                    j1 = 0;
                    k1 = 0;
                    i2 = 1;
                    j2 = 1;
                    k2 = 0;
                }
                else if (x0 >= z0)
                {
                    i1 = 1;
                    j1 = 0;
                    k1 = 0;
                    i2 = 1;
                    j2 = 0;
                    k2 = 1;
                }
                else
                {
                    i1 = 0;
                    j1 = 0;
                    k1 = 1;
                    i2 = 1;
                    j2 = 0;
                    k2 = 1;
                }
            }
            else
            {
                if (y0 < z0)
                {
                    i1 = 0;
                    j1 = 0;
                    k1 = 1;
                    i2 = 0;
                    j2 = 1;
                    k2 = 1;
                }
                else if (x0 < z0)
                {
                    i1 = 0;
                    j1 = 1;
                    k1 = 0;
                    i2 = 0;
                    j2 = 1;
                    k2 = 1;
                }
                else
                {
                    i1 = 0;
                    j1 = 1;
                    k1 = 0;
                    i2 = 1;
                    j2 = 1;
                    k2 = 0;
                }
            }

            double x1 = x0 - i1 + G3; // Offsets for second corner in (x,y,z) coords
            double y1 = y0 - j1 + G3;
            double z1 = z0 - k1 + G3;
            double x2 = x0 - i2 + F3; // Offsets for third corner in (x,y,z) coords
            double y2 = y0 - j2 + F3;
            double z2 = z0 - k2 + F3;
            double x3 = x0 - 0.5; // Offsets for last corner in (x,y,z) coords
            double y3 = y0 - 0.5;
            double z3 = z0 - 0.5;

            // Calculate the contribution from the four corners
            double t0 = 0.6 - x0 * x0 - y0 * y0 - z0 * z0;
            if (t0 > 0)
            {
                t0 *= t0;
                n += t0 * t0 * gradCoord3D(seed, i, j, k, x0, y0, z0);
            }
            double t1 = 0.6 - x1 * x1 - y1 * y1 - z1 * z1;
            if (t1 > 0)
            {
                t1 *= t1;
                n += t1 * t1 * gradCoord3D(seed, i + i1, j + j1, k + k1, x1, y1, z1);
            }
            double t2 = 0.6 - x2 * x2 - y2 * y2 - z2 * z2;
            if (t2 > 0)
            {
                t2 *= t2;
                n += t2 * t2 * gradCoord3D(seed, i + i2, j + j2, k + k2, x2, y2, z2);
            }
            double t3 = 0.6 - x3 * x3 - y3 * y3 - z3 * z3;
            if (t3 > 0)
            {
                t3 *= t3;
                n += t3 * t3 * gradCoord3D(seed, i + 1, j + 1, k + 1, x3, y3, z3);
            }
            // Add contributions from each corner to get the noise value.
            // The result is scaled to stay just inside [-1,1]
            return 31.5 * n;
        }
    }
}
