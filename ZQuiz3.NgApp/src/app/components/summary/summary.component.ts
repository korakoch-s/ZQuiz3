import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Tester } from '../../models/tester';
import { QuizService } from '../../services/quiz.service';

@Component({
  selector: 'app-summary',
  templateUrl: './summary.component.html',
  styleUrls: ['./summary.component.css']
})
export class SummaryComponent implements OnInit {
    public tester: Tester;
    public isWorking: boolean = false;

    constructor(private activeRoute: ActivatedRoute, private quizSvr: QuizService) {
        this.isWorking = true;
        this.activeRoute.params.subscribe(params => {
            this.quizSvr.load(params['username']).then(tester => {
                this.tester = tester;
                this.isWorking = false;
            }).catch(e => {
                console.log('Some error: ' + JSON.stringify(e));
                this.isWorking = false;
            });
        });
    }

    ngOnInit() { }

}
