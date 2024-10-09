import { Component, OnInit, inject } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { DashboardService } from 'src/app/core/services/dashboard.service';
import * as am4core from '@amcharts/amcharts4/core';
import * as am4charts from '@amcharts/amcharts4/charts';
import am4themes_animated from '@amcharts/amcharts4/themes/animated';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  private dashboardService = inject(DashboardService);
  private router = inject(Router);

  totalOrders: number = 0;
  totalCustomers: number = 0;
  totalStorages: number = 0;
  totalEmployees: number = 0;
  totalProducts: number = 0;
  donutChartData=[];
  lineChartData=[]
  private chart!: am4charts.PieChart; // Non-null assertion operator kullanıldı

  data:any;
  ngOnInit(): void {
    this.dashboardService.getAllData().subscribe({
      next: (res) => {
        this.data= res.data;
        this.totalOrders = res.data.totalOrders;
        this.totalCustomers = res.data.totalCustomers;
        this.totalStorages = res.data.totalStorages;
        this.totalEmployees = res.data.totalEmployees;
        this.totalProducts = res.data.totalProducts;
        this.lineChartData = res.data.lineChartData,
        this.donutChartData = res.data.donutChartData;
        // Veriler yüklendikten sonra grafik oluşturma
      this.createDonutChart();
      this.createDurationChart();
      }
    });

    am4core.useTheme(am4themes_animated); // Tema doğru şekilde yüklendi
    this.createDonutChart();
    this.createDurationChart();
  }

  navigateUrl(url: string): void {
    this.router.navigate([url]);
  }

  createDonutChart(): void {
    const chart = am4core.create('donutChart', am4charts.PieChart);
    chart.innerRadius = am4core.percent(40);

    chart.data = this.donutChartData;

    const pieSeries = chart.series.push(new am4charts.PieSeries());
    pieSeries.dataFields.value = 'value';
    pieSeries.dataFields.category = 'category';
    pieSeries.labels.template.text = "{category}: {value.value}"; // labels.template kullanıldı
    pieSeries.labels.template.disabled = false;
    pieSeries.ticks.template.disabled = true;

    // İsteğe bağlı olarak etiket konumu merkezde olsun isterseniz:
    pieSeries.alignLabels = false;
  }

  createDurationChart(): void {
    const chart = am4core.create('durationChart', am4charts.XYChart);
  
    chart.data = this.lineChartData;
  
    const dateAxis = chart.xAxes.push(new am4charts.DateAxis());
    dateAxis.baseInterval = { timeUnit: 'day', count: 1 };
  
    const valueAxis1 = chart.yAxes.push(new am4charts.ValueAxis());
    valueAxis1.title.text = "Sales Amount";
    valueAxis1.renderer.opposite = false;
  
    const valueAxis2 = chart.yAxes.push(new am4charts.ValueAxis());
    valueAxis2.title.text = "Sales Count";
    valueAxis2.renderer.opposite = true;
  
    // İlk seri (value1) - Satış Tutarı
    const series1 = chart.series.push(new am4charts.LineSeries());
    series1.dataFields.valueY = 'totalPrice';
    series1.dataFields.dateX = 'date';
    series1.name = 'Sales Amount';
    series1.tooltipText = '{name}: {valueY}';
    series1.stroke = am4core.color('#DC3545');
    series1.fill = series1.stroke;
    series1.strokeWidth = 2;
    series1.yAxis = valueAxis1;
  
    // İkinci seri (value2) - Satış Rakamı
    const series2 = chart.series.push(new am4charts.ColumnSeries());
    series2.dataFields.valueY = 'orderCount';
    series2.dataFields.dateX = 'date';
    series2.name = 'Sales Count';
    series2.tooltipText = '{name}: {valueY}';
    series2.columns.template.fill = am4core.color('#3F51B5');
    series2.columns.template.stroke = am4core.color('#3F51B5');
    series2.yAxis = valueAxis2;
  
    chart.cursor = new am4charts.XYCursor();
    // chart.scrollbarX = new am4core.Scrollbar();
    // chart.scrollbarY = new am4core.Scrollbar();
  
    // Legend
    chart.legend = new am4charts.Legend();
  }
  
  
}
