import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { QuizService } from '../../services/quiz.service';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
    public userName: string;
    public modalRef: BsModalRef;
    public isWorking: boolean = false;

    constructor(private router: Router, private modalService: BsModalService,
        public quizSvr: QuizService) { }

    ngOnInit() {
    }

    goClick(template: TemplateRef<any>) {
        if (!this.userName || this.userName.length <= 0) {
            console.log('Please input username before go...');
            this.openAlertModal(template);
            event.stopPropagation();
        } else {
            this.isWorking = true;
            let tester = this.quizSvr.register(this.userName).then(tester => {
                this.isWorking = false;
                if (tester.IsCompleted) {
                    this.router.navigate(['/summary', this.userName]);
                } else {
                    this.router.navigate(['/quiz', this.userName]);
                }
            }).catch(e => {
                this.isWorking = false;
                console.log('Error occure: ' + JSON.stringify(e));
            });
        }
    }

    public openAlertModal(template: TemplateRef<any>) {
        this.modalRef = this.modalService.show(template);
    }

}
