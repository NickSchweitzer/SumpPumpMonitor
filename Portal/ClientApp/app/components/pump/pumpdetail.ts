import { Api } from '../../api';
import { inject, computedFrom } from 'aurelia-framework';
import { SumpPump, DataPoint } from '../../models';
import * as chartjs from 'chart.js';

@inject(Api)
export class PumpDetail {
    public pumpId: string;
    public pump: SumpPump;
    public data: DataPoint[];
    public startDate: string;
    public endDate: string;
    private apiClient: Api;

    constructor(api: Api) {
        this.apiClient = api;
    }

    @computedFrom('data')
    get timeScaleOptions(): chartjs.ChartOptions {
        return {
            scales: {
                xAxes: [{
                    type: 'time',
                    time: {
                        minUnit: 'second',
                        tooltipFormat: "h:mm:ss a MM-DD-YYYY",
                        max: this.data[0].timeStamp,
                        min: this.data[this.data.length - 1].timeStamp
                    }
                }]
            },
            legend: {
                display: false
            }
        };
    }

    @computedFrom('data', 'pump')
    get timeScaleData(): chartjs.LinearChartData {

        let lastPoint: DataPoint;
        let currentArray = [];
        let groups = new Array<DataPoint[]>();

        for (var pt of this.data) {
            if (lastPoint != null && lastPoint.pumpRunning != pt.pumpRunning) {
                groups.push(currentArray);
                currentArray = [];
           }
            currentArray.push(pt);
            lastPoint = pt;
        }
        groups.push(currentArray);

        return {
            datasets: groups.map(arr => {
                return {
                    data: arr.map(point => {
                        return {
                            x: point.timeStamp,
                            y: point.waterLevel
                        };
                    }),
                    borderColor: arr[0].pumpRunning ? 'Green' : 'Red'
                };
            })
        };
    }

    search() {
        return this.apiClient.getDataPointsByDate(this.pumpId, this.startDate, this.endDate)
            .then(data => {
                this.data = data;
            });
    }

    activate(params, routeConfig) {
        this.pumpId = params.id;

        return this.apiClient.getPump(this.pumpId)
            .then(data => {
                this.pump = data;
                return this.apiClient.getDataPoints(this.pumpId)
                    .then(data => {
                        this.data = data;
                        this.endDate = this.data[0].timeStamp;
                        this.startDate = this.data[this.data.length - 1].timeStamp;
                    });
            });
    }
}