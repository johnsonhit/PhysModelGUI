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
            
        public static SkiaGraph pressureGraphExp;
        public static SkiaGraph flowGraphExp;
        public static SkiaGraph trendVitalsExp;
        public static SkiaGraph pvLoopGraphExp;

        public static List<AnimatedBloodCompartment> animatedBloodCompartments = new List<AnimatedBloodCompartment>();
        public static List<AnimatedGasComp> animatedGasCompartments = new List<AnimatedGasComp>();
        public static List<AnimatedBloodConnector> animatedBloodConnectors = new List<AnimatedBloodConnector>();
        public static List<AnimatedGasConnector> animatedGasConnectors = new List<AnimatedGasConnector>();
        public static List<AnimatedValve> animatedValves = new List<AnimatedValve>();
        public static List<AnimatedShunt> animatedShunts = new List<AnimatedShunt>();
        public static List<AnimatedShuntGas> animatedShuntsGas = new List<AnimatedShuntGas>();

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

        public static AnimatedShunt AADA;
        public static AnimatedShunt LVRV;
        public static AnimatedShunt LARA;
        public static AnimatedShunt LUNG;

        public static AnimatedShuntGas OUTNCA;
        public static AnimatedBloodCompartment lungs;
        public static AnimatedGasComp alveoli;
        public static AnimatedGasComp ecmolung;

        #region "BuildInterface"
        public static void BuildDiagram()
        {
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
                Name = "AD->ALBLOOD"
            };
            ADALBLOOD.AddConnector(PhysModelMain.FindBloodConnectorByName("AD_ALBLOOD"));
            animatedBloodConnectors.Add(ADALBLOOD);

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
                Name = "ALBLOOD->IVC"
            };
            ALBLOODIVC.AddConnector(PhysModelMain.FindBloodConnectorByName("ALBLOOD_IVC"));
            animatedBloodConnectors.Add(ALBLOODIVC);

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
            pulmonaryValve.AddConnector(PhysModelMain.FindValveByName("RV_PA"));
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
            pulmonaryArtery.AddCompartment(PhysModelMain.FindBloodCompartmentByName("PA"));
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
            PALUNG.AddConnector(PhysModelMain.FindBloodConnectorByName("PA_LL"));
            PALUNG.AddConnector(PhysModelMain.FindBloodConnectorByName("PA_LR"));
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
            aorticValve.AddConnector(PhysModelMain.FindValveByName("LV_AA"));
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
            ascendingAorta.AddCompartment(PhysModelMain.FindBloodCompartmentByName("AA"));
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
            descendingAorta.AddCompartment(PhysModelMain.FindBloodCompartmentByName("AD"));
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
            aorta.AddConnector(PhysModelMain.FindBloodConnectorByName("AD_KIDNEYS"));
            aorta.AddConnector(PhysModelMain.FindBloodConnectorByName("AD_LB"));
            aorta.AddConnector(PhysModelMain.FindBloodConnectorByName("AD_LIVER"));
            animatedBloodConnectors.Add(aorta);

            leftVentricle = new AnimatedBloodCompartment
            {
                scaleRelative = 0.035f,
                Degrees = 0,
                Name = "LV"
            };
            leftVentricle.AddCompartment(PhysModelMain.FindBloodCompartmentByName("LV"));
            animatedBloodCompartments.Add(leftVentricle);

            leftAtrium = new AnimatedBloodCompartment
            {
                scaleRelative = 0.035f,
                Degrees = 340,
                Name = "LA"
            };
            leftAtrium.AddCompartment(PhysModelMain.FindBloodCompartmentByName("LA"));
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
            mitralValve.AddConnector(PhysModelMain.FindValveByName("LA_LV"));
            animatedValves.Add(mitralValve);

            rightVentricle = new AnimatedBloodCompartment
            {
                scaleRelative = 0.035f,
                Degrees = 190,
                Name = "RV"
            };
            rightVentricle.AddCompartment(PhysModelMain.FindBloodCompartmentByName("RV"));
            animatedBloodCompartments.Add(rightVentricle);

            rightAtrium = new AnimatedBloodCompartment
            {
                scaleRelative = 0.035f,
                Degrees = 170,
                Name = "RA"
            };
            rightAtrium.AddCompartment(PhysModelMain.FindBloodCompartmentByName("RA"));
            animatedBloodCompartments.Add(rightAtrium);

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
            tricuspidValve.AddConnector(PhysModelMain.FindValveByName("RA_RV"));
            animatedValves.Add(tricuspidValve);


            lowerBody = new AnimatedBloodCompartment
            {
                scaleRelative = 0.035f,
                Degrees = 90,
                Name = "LB"
            };
            lowerBody.AddCompartment(PhysModelMain.FindBloodCompartmentByName("LIVER"));
            lowerBody.AddCompartment(PhysModelMain.FindBloodCompartmentByName("KIDNEYS"));
            lowerBody.AddCompartment(PhysModelMain.FindBloodCompartmentByName("LB"));
            animatedBloodCompartments.Add(lowerBody);

            lungs = new AnimatedBloodCompartment
            {
                scaleRelative = 0.035f,
                Degrees = 270,
                Name = "LUNG"
            };
            lungs.AddCompartment(PhysModelMain.FindBloodCompartmentByName("LL"));
            lungs.AddCompartment(PhysModelMain.FindBloodCompartmentByName("LR"));
            animatedBloodCompartments.Add(lungs);



            upperBody = new AnimatedBloodCompartment
            {
                scaleRelative = 0.035f,
                Degrees = 90,
                RadiusYOffset = 0.5f,
                Name = "UB"
            };
            upperBody.AddCompartment(PhysModelMain.FindBloodCompartmentByName("BRAIN"));
            upperBody.AddCompartment(PhysModelMain.FindBloodCompartmentByName("UB"));
            animatedBloodCompartments.Add(upperBody);

            alveoli = new AnimatedGasComp
            {
                scaleRelative = 0.035f,
                Degrees = 270,
                RadiusYOffset = 1f,
                Name = "ALV"
            };
            alveoli.AddCompartment(PhysModelMain.FindGasCompartmentByName("ALL"));
            alveoli.AddCompartment(PhysModelMain.FindGasCompartmentByName("ALR"));
            animatedGasCompartments.Add(alveoli);

            ecmolung = new AnimatedGasComp
            {
                scaleRelative = 0.035f,
                Degrees = 70,
                RadiusYOffset = 0.75f,
                Name = "ECMO"
            };
            ecmolung.AddCompartment(PhysModelMain.FindGasCompartmentByName("ALGAS"));
            animatedGasCompartments.Add(ecmolung);


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
            LBIVC.AddConnector(PhysModelMain.FindBloodConnectorByName("LB_IVC"));
            LBIVC.AddConnector(PhysModelMain.FindBloodConnectorByName("KIDNEYS_IVC"));
            LBIVC.AddConnector(PhysModelMain.FindBloodConnectorByName("LIVER_IVC"));
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
            IVC.AddCompartment(PhysModelMain.FindBloodCompartmentByName("IVC"));
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
            IVCRA.AddConnector(PhysModelMain.FindBloodConnectorByName("IVC_RA"));
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
            UBSVC.AddConnector(PhysModelMain.FindBloodConnectorByName("UB_SVC"));
            UBSVC.AddConnector(PhysModelMain.FindBloodConnectorByName("BRAIN_SVC"));
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
            SVC.AddCompartment(PhysModelMain.FindBloodCompartmentByName("SVC"));
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
            SVCRA.AddConnector(PhysModelMain.FindBloodConnectorByName("SVC_RA"));
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
            LUNGPV.AddConnector(PhysModelMain.FindBloodConnectorByName("LL_PV"));
            LUNGPV.AddConnector(PhysModelMain.FindBloodConnectorByName("LR_PV"));
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
            pulmonaryVeins.AddCompartment(PhysModelMain.FindBloodCompartmentByName("PV"));
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
            PVLA.AddConnector(PhysModelMain.FindBloodConnectorByName("PV_LA"));
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
            AAUB.AddConnector(PhysModelMain.FindBloodConnectorByName("AA_UB"));
            AAUB.AddConnector(PhysModelMain.FindBloodConnectorByName("AA_BRAIN"));
            animatedBloodConnectors.Add(AAUB);


            AADA = new AnimatedShunt
            {
                scaleRelative = 0.035f,
                RadiusYOffset = 1f,
                NoLoss = false,
                StartAngle = 25,
                IsVisible = true,
                EndAngle = 225,
                Direction = 1,
                Name = "DUCTUS ARTERIOSUS"
            };
            AADA.AddConnector(PhysModelMain.FindBloodConnectorByName("DA_PA"));
            AADA.AddConnector(PhysModelMain.FindBloodConnectorByName("AD_DA"));
            animatedShunts.Add(AADA);

            LVRV = new AnimatedShunt
            {
                scaleRelative = 0.035f,
                RadiusYOffset = 1f,
                NoLoss = true,
                IsVisible = false,
                StartAngle = 0,
                EndAngle = 190,
                Direction = 1,
                Name = "VSD"
            };
            LVRV.AddConnector(PhysModelMain.FindBloodConnectorByName("LV_RV"));
            animatedShunts.Add(LVRV);

            LARA = new AnimatedShunt
            {
                scaleRelative = 0.035f,
                RadiusYOffset = 1f,
                NoLoss = true,
                IsVisible = false,
                StartAngle = 340,
                EndAngle = 170,
                Direction = 1,
                Name = "FORAMEN OVALE"
            };
            LARA.AddConnector(PhysModelMain.FindBloodConnectorByName("LA_RA"));
            animatedShunts.Add(LARA);

            LUNG = new AnimatedShunt
            {
                scaleRelative = 0.035f,
                RadiusYOffset = 1f,
                NoLoss = true,
                IsVisible = false,
                StartAngle = 315,
                EndAngle = 225,
                Direction = 1,
                Name = "LUNG SHUNT"
            };
            LUNG.AddConnector(PhysModelMain.FindBloodConnectorByName("PA_PV"));
            animatedShunts.Add(LUNG);

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
            OUTNCA.AddConnector(PhysModelMain.FindGasConnectorByName("OUT_NCA"));
            animatedShuntsGas.Add(OUTNCA);

            ALBLOOD = new AnimatedBloodCompartment
            {
                scaleRelative = 0.035f,
                IsVessel = false,
                RadiusYOffset = 0.75f,
                OffsetXFactor = 4f,
                StartAngle = 80,
                EndAngle = 100,
                Degrees = 70,
                Name = "ECMO"
            };
            ALBLOOD.AddCompartment(PhysModelMain.FindBloodCompartmentByName("ALBLOOD"));
            animatedBloodCompartments.Add(ALBLOOD);

           
       


        }
        public static void BuildScrollingGraphs()
        {
            pressureGraphExp = new SkiaGraph();
            flowGraphExp = new SkiaGraph();
            trendVitalsExp = new SkiaGraph()
            {
                MaxY = 220,
                MinY = 0,
                IsSideScrolling = true,
                HideXAxisLabels = false,
                Stepsize = 1,
                SourceDataResolution = 1f, // datapoints per second
                GridYAxisStep = 20,
                GridXAxisStep = 60,
                TimeBasedInMinutes = true,
                Legend1 = "HR",
                Legend2 = "SAT",
                Legend3 = "SYST",
                Legend4 = "DIAST",
                Legend5 = "RESP",
                Graph1Enabled = true,
                Graph2Enabled = true,
                Graph3Enabled = true,
                Graph4Enabled = true,
                Graph5Enabled = true
            };
            pvLoopGraphExp = new SkiaGraph()
            {
                Stepsize = 1,
                MaxY = 90,
                MinY = 60,
                MinX = 0,
                MaxX = 20,
                GridYAxisStep = 5,
                GridXAxisStep = 5,
                HideXAxisLabels = false,
                IsSideScrolling = false,
                SourceDataResolution = 1f / 0.015f,
                RefreshRate = 100,
                PointMode1 = SKPointMode.Points
            };


           

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
        public static void DrawPVLoopGraphGrid(SKCanvas _canvas, float _width, float _height)
        {
            if (pvLoopGraphExp != null)
            {

                pvLoopGraphExp.DrawGrid(_canvas, (int)_width, (int)_height);
            }
        }
        public static void DrawTrendGraphVitals(SKCanvas _canvas, float _width, float _height)
        {
            if (trendVitalsExp != null)
            {
                trendVitalsExp.DrawGraph(_canvas, (int)_width, (int)_height);
            }
        }
        public static void DrawTrendGraphVitalsGrid(SKCanvas _canvas, float _width, float _height)
        {
            if (trendVitalsExp != null)
            {
                trendVitalsExp.DrawGrid(_canvas, (int)_width, (int)_height);
            }
        }
        public static void DrawPressureCurve(SKCanvas _canvas, float _width, float _height)
        {
            if (pressureGraphExp != null)
            {
                pressureGraphExp.DrawGraph(_canvas, _width, _height);
            }
        }
        public static void DrawPressureCurveGrid(SKCanvas _canvas, float _width, float _height)
        {

            if (pressureGraphExp != null)
            {
                pressureGraphExp.DrawGrid(_canvas, _width, _height);

            }
        }
        public static void DrawFlowCurve(SKCanvas _canvas, float _width, float _height)
        {
            if (flowGraphExp != null)
            {
                flowGraphExp.DrawGraph(_canvas, (int)_width, (int)_height);
            }
        }
        public static void DrawFlowCurveGrid(SKCanvas _canvas, float _width, float _height)
        {
            if (flowGraphExp != null)
            {
                flowGraphExp.DrawGrid(_canvas, (int)_width, (int)_height);
            }
        }
        public static void DrawPVLoop(SKCanvas _canvas, float _width, float _height)
        {
            if (pvLoopGraphExp != null)
            {

                pvLoopGraphExp.DrawGraph(_canvas, (int)_width, (int)_height);
            }
        }
        public static void PDAToggle()
        {
            if (AADA != null)
            {
                if (AADA.IsVisible)
                {
                    AADA.IsVisible = false;
                }
                else
                {
                    AADA.IsVisible = true;
                }
            }
        }
        public static void VSDToggle()
        {
            if (LVRV != null)
            {
                if (LVRV.IsVisible)
                {
                    LVRV.IsVisible = false;
                }
                else
                {
                    LVRV.IsVisible = true;
                }
            }
        }
        public static void OFOToggle()
        {
            if (LARA != null)
            {
                if (LARA.IsVisible)
                {
                    LARA.IsVisible = false;
                }
                else
                {
                    LARA.IsVisible = true;
                }
            }
        }
        public static void LUNGSHUNTToggle()
        {
            if (LUNG != null)
            {
                if (LUNG.IsVisible)
                {
                    LUNG.IsVisible = false;
                }
                else
                {
                    LUNG.IsVisible = true;
                }
            }
        }

        #endregion





    }
}
