import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { ModalModule } from 'ngx-bootstrap/modal';

import { QuizService } from './services/quiz.service';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { RegisterComponent } from './components/register/register.component';
import { QuizComponent } from './components/quiz/quiz.component';
import { SummaryComponent } from './components/summary/summary.component';
import { QuizItemComponent } from './components/quiz-item/quiz-item.component';

@NgModule({
    declarations: [
        AppComponent,
        RegisterComponent,
        QuizComponent,
        SummaryComponent,
        QuizItemComponent
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        FormsModule,
        ModalModule.forRoot(),
        HttpClientModule
    ],
    providers: [
        { provide: 'API_URL', useValue: 'http://localhost:54263/api/'},
        QuizService
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
