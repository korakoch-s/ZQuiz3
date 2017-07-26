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

    constructor(private activeRoute: ActivatedRoute, private quizSvr: QuizService) {
        this.activeRoute.params.subscribe(params => {
            this.quizSvr.load(params['username']).then(tester => {
                this.tester = tester;
            });
        });
    }

    ngOnInit() { }

}
