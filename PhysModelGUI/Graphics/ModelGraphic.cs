using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SkiaSharp;
using PhysModelLibrary.Connectors;
using PhysModelLibrary;

namespace PhysModelGUI
{
    public static class ModelGraphic
    {
        static readonly SKPaint paint = new SKPaint()
        {
            Style = SKPaintStyle.Fill,
            IsAntialias = false,
            Color = SKColors.DarkGray,
            StrokeWidth = 10
        };
        static readonly SKPaint airwayPaint = new SKPaint()
        {
            Style = SKPaintStyle.Fill,
            IsAntialias = true,
            Color = SKColors.DarkGray,
            StrokeWidth = 25,
        };
        static readonly SKPaint airwayBronchi = new SKPaint()
        {
            Style = SKPaintStyle.Stroke,
            IsAntialias = true,
            Color = SKColors.DarkGray,
            StrokeWidth = 25,
        };
            

        public static List<AnimatedBloodCompartment> animatedBloodCompartments = new List<AnimatedBloodCompartment>();
        public static List<AnimatedGasComp> animatedGasCompartments = new List<AnimatedGasComp>();
        public static List<AnimatedBloodConnector> animatedBloodConnectors = new List<AnimatedBloodConnector>();
        public static List<AnimatedGasConnector> animatedGasConnectors = new List<AnimatedGasConnector>();
        public static List<AnimatedValve> animatedValves = new List<AnimatedValve>();
        public static List<AnimatedShunt> animatedShunts = new List<AnimatedShunt>();
        public static List<AnimatedShuntGas> animatedShuntsGas = new List<AnimatedShuntGas>();

        public static AnimatedBloodCompartment myocardium;
        public static AnimatedBloodConnector AAMYO;
        public static AnimatedBloodConnector MYORA;
        public static AnimatedBloodConnector LUNGPV;
        public static AnimatedBloodCompartment pulmonaryVeins;
        public static AnimatedBloodConnector PVLA;
        public static AnimatedBloodCompartment leftAtrium;
        public static AnimatedValve mitralValve;
        public static AnimatedBloodCompartment leftVentricle;
        public static AnimatedValve aorticValve;
        public static AnimatedBloodCompartment ascendingAorta;
        public static AnimatedBloodCompartment descendingAorta;
        public static AnimatedBloodConnector aorta;
        public static AnimatedBloodCompartment lowerBody;
        public static AnimatedBloodConnector LBIVC;
        public static AnimatedBloodCompartment IVC;
        public static AnimatedBloodConnector IVCRA;
        public static AnimatedBloodCompartment rightAtrium;
        public static AnimatedValve tricuspidValve;
        public static AnimatedBloodCompartment rightVentricle;
        public static AnimatedValve pulmonaryValve;
        public static AnimatedBloodCompartment pulmonaryArtery;
        public static AnimatedBloodConnector PALUNG;
        public static AnimatedBloodCompartment ALBLOOD;
        public static AnimatedBloodConnector ADALBLOOD;
        public static AnimatedBloodConnector ALBLOODIVC;

        public static AnimatedBloodConnector AAUB;
        public static AnimatedBloodCompartment upperBody;
        public static AnimatedBloodConnector UBSVC;
        public static AnimatedBloodCompartment SVC;
        public static AnimatedBloodConnector SVCRA;

        public static AnimatedShunt PDA;
        public static AnimatedShunt VSD;
        public static AnimatedShunt OFO;
        public static AnimatedShunt LUNGSHUNT;

        public static AnimatedShuntGas OUTNCA;
        public static AnimatedBloodCompartment lungs;
        public static AnimatedGasComp alveoli;
        public static AnimatedGasComp placenta;

        public static AnimatedBloodCompartment lvad;
        public static AnimatedBloodCompartment rvad;
        public static AnimatedBloodCompartment ecmopump;
        public static AnimatedBloodCompartment ecmolungblood;
        public static AnimatedGasComp ecmolunggas;


        #region "BuildInterface"

        public static void ClearLists()
        {
            animatedBloodCompartments.Clear();
            animatedGasCompartments.Clear();
            animatedBloodConnectors.Clear();
            animatedGasConnectors.Clear();
            animatedValves.Clear();
            animatedShunts.Clear();
            animatedShuntsGas.Clear();

        }
        public static void BuildDiagram()
        {

            ClearLists();

            ADALBLOOD = new AnimatedBloodConnector
            {
                scaleRelative = 0.035f,
                Degrees = 0,
                NoLoss = true,
                RadiusYOffset = 0.75f,
                RadiusXOffset = 1.3f,
                StartAngle = 50,
                EndAngle = 70,
                Direction = 1,
                IsVisible = false,
                Name = "AD->ALBLOOD"
            };
            ADALBLOOD.AddConnector(PhysModelMain.currentModel.AD_ALBLOOD);

            ALBLOODIVC = new AnimatedBloodConnector
            {
                scaleRelative = 0.035f,
                Degrees = 0,
                NoLoss = true,
                RadiusYOffset = 0.75f,
                RadiusXOffset = 1.3f,
                StartAngle = 70,
                EndAngle = 128,
                Direction = 1,
                IsVisible = false,
                Name = "ALBLOOD->IVC"
            };
            ALBLOODIVC.AddConnector(PhysModelMain.currentModel.ALBLOOD_IVC);

            pulmonaryValve = new AnimatedValve
            {
                scaleRelative = 0.035f,
                Degrees = 0,
                NoLoss = false,
                RadiusYOffset = 1f,
                StartAngle = 190,
                EndAngle = 220,
                Direction = 1,
                Name = "PV"
            };
            pulmonaryValve.AddConnector(PhysModelMain.currentModel.RV_PA);
            animatedValves.Add(pulmonaryValve);

            pulmonaryArtery = new AnimatedBloodCompartment
            {
                scaleRelative = 0.035f,
                IsVessel = true,
                StartAngle = 220,
                EndAngle = 230,
                Degrees = 225,
                Name = "PA"
            };
            pulmonaryArtery.AddCompartment(PhysModelMain.currentModel.PA);
            animatedBloodCompartments.Add(pulmonaryArtery);

            PALUNG = new AnimatedBloodConnector
            {
                scaleRelative = 0.035f,
                RadiusYOffset = 1f,
                NoLoss = true,
                StartAngle = 230,
                EndAngle = 270,
                Direction = 1,
                Name = "PA_LUNG"
            };
            PALUNG.AddConnector(PhysModelMain.currentModel.PA_LL);
            PALUNG.AddConnector(PhysModelMain.currentModel.PA_LR);
            animatedBloodConnectors.Add(PALUNG);

            aorticValve = new AnimatedValve
            {
                scaleRelative = 0.035f,
                Degrees = 0,
                NoLoss = false,
                RadiusYOffset = 1f,
                StartAngle = 0,
                EndAngle = 15,
                Direction = 1,
                Name = "AV"
            };
            aorticValve.AddConnector(PhysModelMain.currentModel.LV_AA);
            animatedValves.Add(aorticValve);

            ascendingAorta = new AnimatedBloodCompartment
            {
                scaleRelative = 0.035f,
                IsVessel = true,
                RadiusYOffset = 1f,
                StartAngle = 25,
                EndAngle = 15,
                Degrees = 20,
                OffsetXFactor = 0.75f,
                Direction = -1,
                Name = "AA"
            };
            ascendingAorta.AddCompartment(PhysModelMain.currentModel.AA);
            animatedBloodCompartments.Add(ascendingAorta);

            descendingAorta = new AnimatedBloodCompartment
            {
                scaleRelative = 0.035f,
                IsVessel = true,
                RadiusYOffset = 1f,
                StartAngle = 35,
                EndAngle = 25,
                Degrees = 30,
                Direction = -1,
                Name = "AD"
            };
            descendingAorta.AddCompartment(PhysModelMain.currentModel.AD);
            animatedBloodCompartments.Add(descendingAorta);

            aorta = new AnimatedBloodConnector
            {
                scaleRelative = 0.035f,
                Degrees = 0,
                NoLoss = true,
                RadiusYOffset = 1f,
                StartAngle = 35,
                EndAngle = 90,
                Direction = 1,
                Name = "AD->LB"
            };
            aorta.AddConnector(PhysModelMain.currentModel.AD_KIDNEYS);
            aorta.AddConnector(PhysModelMain.currentModel.AD_LB);
            aorta.AddConnector(PhysModelMain.currentModel.AD_LIVER);
            animatedBloodConnectors.Add(aorta);

            myocardium = new AnimatedBloodCompartment
            {
                scaleRelative = 0.035f,
                Degrees = 90,
                RadiusYOffset = 0.725f,
                Name = "MYO"
            };
            myocardium.AddCompartment(PhysModelMain.currentModel.MYO);

            leftVentricle = new AnimatedBloodCompartment
            {
                scaleRelative = 0.035f,
                Degrees = 0,
               
                Name = "LV"
            };
            leftVentricle.AddCompartment(PhysModelMain.currentModel.LV);
            animatedBloodCompartments.Add(leftVentricle);

            lvad = new AnimatedBloodCompartment
            {
                scaleRelative = 0.035f,
                Degrees = 10,
                Name = "LVAD"
            };
            lvad.AddCompartment(PhysModelMain.currentModel.LVAD);

            leftAtrium = new AnimatedBloodCompartment
            {
                scaleRelative = 0.035f,
                Degrees = 340,
                Name = "LA"
            };
            leftAtrium.AddCompartment(PhysModelMain.currentModel.LA);
            animatedBloodCompartments.Add(leftAtrium);

            mitralValve = new AnimatedValve
            {
                scaleRelative = 0.035f,
                Degrees = 0,
                RadiusYOffset = 1f,
                StartAngle = 340,
                EndAngle = 360,
                Direction = 1,
                Name = "TV"
            };
            mitralValve.AddConnector(PhysModelMain.currentModel.LA_LV);
            animatedValves.Add(mitralValve);

            rightVentricle = new AnimatedBloodCompartment
            {
                scaleRelative = 0.035f,
                Degrees = 190,
                Name = "RV"
            };
            rightVentricle.AddCompartment(PhysModelMain.currentModel.RV);
            animatedBloodCompartments.Add(rightVentricle);

            rightAtrium = new AnimatedBloodCompartment
            {
                scaleRelative = 0.035f,
                Degrees = 170,
                Name = "RA"
            };
            rightAtrium.AddCompartment(PhysModelMain.currentModel.RA);
            animatedBloodCompartments.Add(rightAtrium);

            rvad = new AnimatedBloodCompartment
            {
                scaleRelative = 0.035f,
                Degrees = 200,
                IsVisible = false,

                Name = "RVAD"
            };
            rvad.AddCompartment(PhysModelMain.currentModel.RVAD);

            tricuspidValve = new AnimatedValve
            {
                scaleRelative = 0.035f,
                Degrees = 0,
                RadiusYOffset = 1f,
                StartAngle = 170,
                EndAngle = 190,
                Direction = 1,
                Name = "TV"
            };
            tricuspidValve.AddConnector(PhysModelMain.currentModel.RA_RV);
            animatedValves.Add(tricuspidValve);

            lowerBody = new AnimatedBloodCompartment
            {
                scaleRelative = 0.035f,
                Degrees = 90,
                Name = "LB"
            };
            lowerBody.AddCompartment(PhysModelMain.currentModel.LIVER);
            lowerBody.AddCompartment(PhysModelMain.currentModel.KIDNEYS);
            lowerBody.AddCompartment(PhysModelMain.currentModel.LB);
            animatedBloodCompartments.Add(lowerBody);

            lungs = new AnimatedBloodCompartment
            {
                scaleRelative = 0.035f,
                Degrees = 270,
                Name = "LUNG"
            };
            lungs.AddCompartment(PhysModelMain.currentModel.LL);
            lungs.AddCompartment(PhysModelMain.currentModel.LR);
            animatedBloodCompartments.Add(lungs);

            upperBody = new AnimatedBloodCompartment
            {
                scaleRelative = 0.035f,
                Degrees = 90,
                RadiusYOffset = 0.5f,
                Name = "UB"
            };
            upperBody.AddCompartment(PhysModelMain.currentModel.BRAIN);
            upperBody.AddCompartment(PhysModelMain.currentModel.UB);
            animatedBloodCompartments.Add(upperBody);

            alveoli = new AnimatedGasComp
            {
                scaleRelative = 0.035f,
                Degrees = 270,
                RadiusYOffset = 1f,
                Name = "ALV"
            };
            alveoli.AddCompartment(PhysModelMain.currentModel.ALL);
            alveoli.AddCompartment(PhysModelMain.currentModel.ALR);
            animatedGasCompartments.Add(alveoli);

            placenta = new AnimatedGasComp
            {
                scaleRelative = 0.035f,
                Degrees = 70,
                RadiusYOffset = 0.75f,
                Name = "ECMO",
                IsVisible = false,
            };
            placenta.AddCompartment(PhysModelMain.currentModel.ALGAS);

            ecmolunggas = new AnimatedGasComp
            {
                scaleRelative = 0.035f,
                Degrees = 70,
                RadiusYOffset = 0.75f,
                Name = "ECMO",
                IsVisible = false,
            };
            ecmolunggas.AddCompartment(PhysModelMain.currentModel.ECLUNGGAS);

            ecmolungblood = new AnimatedBloodCompartment
            {
                scaleRelative = 0.035f,
                Degrees = 70,
                RadiusYOffset = 0.75f,
                Name = "ECMO",
                IsVisible = false,
            };
            ecmolungblood.AddCompartment(PhysModelMain.currentModel.ECLUNGBLOOD);

            LBIVC = new AnimatedBloodConnector
            {
                scaleRelative = 0.035f,
                Degrees = 0,
                NoLoss = false,
                RadiusYOffset = 1f,
                StartAngle = 90,
                EndAngle = 120,
                Direction = 1,
                Name = "LB->IVC"
            };
            LBIVC.AddConnector(PhysModelMain.currentModel.LB_IVC);
            LBIVC.AddConnector(PhysModelMain.currentModel.KIDNEYS_IVC);
            LBIVC.AddConnector(PhysModelMain.currentModel.LIVER_IVC);
            animatedBloodConnectors.Add(LBIVC);

            IVC = new AnimatedBloodCompartment
            {
                scaleRelative = 0.035f,
                IsVessel = true,
                StartAngle = 130,
                EndAngle = 120,
                Degrees = 125,
                OffsetXFactor = 4f,
                Direction = -1,
                Name = "IVC"
            };
            IVC.AddCompartment(PhysModelMain.currentModel.IVC);
            animatedBloodCompartments.Add(IVC);

            IVCRA = new AnimatedBloodConnector
            {
                scaleRelative = 0.035f,
                RadiusYOffset = 1f,
                NoLoss = true,
                StartAngle = 130,
                EndAngle = 170,
                Direction = 1,
                Name = "IVC->RA"
            };
            IVCRA.AddConnector(PhysModelMain.currentModel.IVC_RA);
            animatedBloodConnectors.Add(IVCRA);

            UBSVC = new AnimatedBloodConnector
            {
                scaleRelative = 0.035f,
                Degrees = 0,
                NoLoss = false,
                RadiusYOffset = 0.5f,
                StartAngle = 90,
                EndAngle = 120,
                Direction = 1,
                Name = "UB->SVC"
            };
            UBSVC.AddConnector(PhysModelMain.currentModel.UB_SVC);
            UBSVC.AddConnector(PhysModelMain.currentModel.BRAIN_SVC);
            animatedBloodConnectors.Add(UBSVC);

            SVC = new AnimatedBloodCompartment
            {
                scaleRelative = 0.035f,
                IsVessel = true,
                RadiusYOffset = 0.5f,
                StartAngle = 130,
                OffsetXFactor = 5f,
                EndAngle = 120,
                Direction = -1,
                Degrees = 125,
                Name = "SVC"
            };
            SVC.AddCompartment(PhysModelMain.currentModel.SVC);
            animatedBloodCompartments.Add(SVC);

            SVCRA = new AnimatedBloodConnector
            {
                scaleRelative = 0.035f,
                RadiusYOffset = 0.5f,
                RadiusXOffset = 1.05f,
                NoLoss = true,
                StartAngle = 130,
                EndAngle = 165,
                Direction = 1,
                Name = "SVC->RA"
            };
            SVCRA.AddConnector(PhysModelMain.currentModel.SVC_RA);
            animatedBloodConnectors.Add(SVCRA);


            LUNGPV = new AnimatedBloodConnector
            {
                scaleRelative = 0.035f,
                RadiusYOffset = 1f,
                NoLoss = false,
                StartAngle = 270,
                EndAngle = 310,
                Direction = 1,
                Name = "LUNG->PV"
            };
            LUNGPV.AddConnector(PhysModelMain.currentModel.LL_PV);
            LUNGPV.AddConnector(PhysModelMain.currentModel.LR_PV);
            animatedBloodConnectors.Add(LUNGPV);

            pulmonaryVeins = new AnimatedBloodCompartment
            {
                scaleRelative = 0.035f,
                IsVessel = true,
                StartAngle = 310,
                EndAngle = 320,
                Degrees = 315,
                Name = "PV"
            };
            pulmonaryVeins.AddCompartment(PhysModelMain.currentModel.PV);
            animatedBloodCompartments.Add(pulmonaryVeins);

            PVLA = new AnimatedBloodConnector
            {
                scaleRelative = 0.035f,
                RadiusYOffset = 1f,
                StartAngle = 320,
                EndAngle = 340,
                Direction = 1,
                Name = "PV->LA"
            };
            PVLA.AddConnector(PhysModelMain.currentModel.PV_LA);
            animatedBloodConnectors.Add(PVLA);

            AAUB = new AnimatedBloodConnector
            {
                scaleRelative = 0.035f,
                RadiusYOffset = 0.5f,
                RadiusXOffset = 1.3f,
                NoLoss = true,
                StartAngle = 40,
                EndAngle = 90,
                Direction = 1,
                Name = "AA->UB"
            };
            AAUB.AddConnector(PhysModelMain.currentModel.AA_UB);
            AAUB.AddConnector(PhysModelMain.currentModel.AA_BRAIN);
            animatedBloodConnectors.Add(AAUB);


            PDA = new AnimatedShunt
            {
                scaleRelative = 0.035f,
                RadiusYOffset = 1f,
                NoLoss = false,
                StartAngle = 25,
                EndAngle = 225,
                Direction = 1,
                Name = "DUCTUS ARTERIOSUS"
            };
            PDA.AddConnector(PhysModelMain.currentModel.DA_PA);
            PDA.AddConnector(PhysModelMain.currentModel.AD_DA);
       

            VSD = new AnimatedShunt
            {
                scaleRelative = 0.035f,
                RadiusYOffset = 1f,
                NoLoss = true,
                StartAngle = 0,
                EndAngle = 190,
                Direction = 1,
                Name = "VSD"
            };
            VSD.AddConnector(PhysModelMain.currentModel.LV_RV);
 

            OFO = new AnimatedShunt
            {
                scaleRelative = 0.035f,
                RadiusYOffset = 1f,
                NoLoss = true,
                StartAngle = 340,
                EndAngle = 170,
                Direction = 1,
                Name = "FORAMEN OVALE"
            };
            OFO.AddConnector(PhysModelMain.currentModel.LA_RA);
     

            LUNGSHUNT = new AnimatedShunt
            {
                scaleRelative = 0.035f,
                RadiusYOffset = 1f,
                NoLoss = true,
                StartAngle = 315,
                EndAngle = 225,
                Direction = 1,
                Name = "LUNG SHUNT"
            };
            LUNGSHUNT.AddConnector(PhysModelMain.currentModel.PA_PV);


            OUTNCA = new AnimatedShuntGas
            {
                scaleRelative = 0.035f,
                RadiusYOffset = 1.45f,
                RadiusXOffset = 0f,
                NoLoss = true,
                StartAngle = 270,
                EndAngle = 230,
                Direction = 1,
                Name = "AIRWAY"
            };
            OUTNCA.AddConnector(PhysModelMain.currentModel.OUT_NCA);
            //animatedShuntsGas.Add(OUTNCA);

            AAMYO = new AnimatedBloodConnector
            {
                scaleRelative = 0.035f,
                Degrees = 0,
                NoLoss = true,
                RadiusYOffset = 0.725f,
                StartAngle = 20,
                EndAngle = 90,
                Direction = 1,
                Name = "AA->MYO"
            };
            AAMYO.AddConnector(PhysModelMain.currentModel.AA_MYO);

            MYORA = new AnimatedBloodConnector
            {
                scaleRelative = 0.035f,
                Degrees = 0,
                NoLoss = true,
                RadiusYOffset = 0.725f,
                StartAngle = 90,
                EndAngle = 170,
                Direction = 1,
                Name = "MYO->RA"
            };
            MYORA.AddConnector(PhysModelMain.currentModel.MYO_RA);

            ALBLOOD = new AnimatedBloodCompartment
            {
                scaleRelative = 0.035f,
                IsVessel = false,
                RadiusYOffset = 0.75f,
                OffsetXFactor = 4f,
                StartAngle = 80,
                EndAngle = 100,
                Degrees = 70,
                IsVisible = false,
                Name = "ECMO"
            };
            ALBLOOD.AddCompartment(PhysModelMain.currentModel.ALBLOOD);
        }

        public static void DrawMainDiagram(SKCanvas _canvas, float _width, float _height)
        {
            _canvas.Clear(SKColors.Transparent);
            _canvas.Translate(_width / 2, _height / 2);

            foreach (AnimatedBloodConnector ac in animatedBloodConnectors)
            {
                if (ac.IsVisible)
                    ac.DrawConnector(_canvas, _width, _height);
            }

            foreach (AnimatedGasConnector ac in animatedGasConnectors)
            {
                if (ac.IsVisible)
                    ac.DrawConnector(_canvas, _width, _height);
            }

            foreach (AnimatedValve av in animatedValves)
            {
                if (av.IsVisible)
                    av.DrawConnector(_canvas, _width, _height);
            }

            foreach (AnimatedShunt ash in animatedShunts)
            {
                if (ash.IsVisible)
                    ash.DrawConnector(_canvas, _width, _height);
            }

            foreach (AnimatedShuntGas ashg in animatedShuntsGas)
            {
                if (ashg.IsVisible)
                    ashg.DrawConnector(_canvas, _width, _height);
            }

            foreach (AnimatedGasComp ag in animatedGasCompartments)
            {
                if (ag.IsVisible)
                    ag.DrawCompartment(_canvas, _width, _height);
            }

            foreach (AnimatedBloodCompartment ab in animatedBloodCompartments)
            {
                if (ab.IsVisible)
                    ab.DrawCompartment(_canvas, _width, _height);
            }
        }
        public static void DrawDiagramSkeleton(SKCanvas _canvas, float _width, float _height)
        {
            _canvas.Clear(SKColors.Transparent);
            _canvas.Translate(_width / 2, _height / 2);

            // draw main circle
            SKPoint location = new SKPoint(0, 0);
            float radius = _width / 2.5f;
            if (_width > _height)
            {
                radius = _height / 2.5f;
            }

            _canvas.DrawCircle(location, radius, paint);

        }

        public static void PDAView(bool state)
        {
            if (state)
            {
                if (!animatedShunts.Contains(PDA))
                    animatedShunts.Add(PDA);
            }
            else
            {
                if (animatedShunts.Contains(PDA))
                    animatedShunts.Remove(PDA);
            }
        }
        public static void VSDView(bool state)
        {
            if (state)
            {
                if (!animatedShunts.Contains(VSD))
                    animatedShunts.Add(VSD);
            }
            else
            {
                if (animatedShunts.Contains(VSD))
                    animatedShunts.Remove(VSD);
            }
        }
        public static void OFOView(bool state)
        {
            if (state)
            {
                if (!animatedShunts.Contains(OFO))
                    animatedShunts.Add(OFO);
            }
            else
            {
                if (animatedShunts.Contains(OFO))
                    animatedShunts.Remove(OFO);
            }
        }
        public static void LUNGSHUNTView(bool state)
        {
            if (state)
            {
                if (!animatedShunts.Contains(LUNGSHUNT))
                    animatedShunts.Add(LUNGSHUNT);
            } else
            {
                if (animatedShunts.Contains(LUNGSHUNT))
                animatedShunts.Remove(LUNGSHUNT);
            }
        }

        public static void MYOView(bool state)
        {
            if (state)
            {
                if (!animatedBloodCompartments.Contains(myocardium))
                    animatedBloodCompartments.Add(myocardium);
                if (!animatedBloodConnectors.Contains(MYORA))
                    animatedBloodConnectors.Insert(0, MYORA);
                if (!animatedBloodConnectors.Contains(AAMYO))
                    animatedBloodConnectors.Insert(1,AAMYO);
            }
            else
            {
                animatedBloodCompartments.Remove(myocardium);
                animatedBloodConnectors.Remove(AAMYO);
                animatedBloodConnectors.Remove(MYORA);
            }
        }

        #endregion





    }
}
