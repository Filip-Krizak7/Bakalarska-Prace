import "./PastPracticeListComponent.css";
import { parseISO, format } from 'date-fns';
import Accordion from "react-bootstrap/Accordion";
import React, {useEffect, useState} from "react";
import DLImage from "../../../resources/DLImg.svg";
import {Col, Container, Modal, OverlayTrigger, Row, Tooltip} from "react-bootstrap";
import {axios} from "../../../axios.js";
import {BsExclamationTriangleFill, BsFillXCircleFill, BsInfoCircleFill, BsSearch, BsSliders} from "react-icons/bs";
import Badge from "react-bootstrap/Badge";
import Combobox from "react-widgets/Combobox";
import "react-widgets/styles.css";
import 'react-date-range/dist/styles.css'; // main style file
import 'react-date-range/dist/theme/default.css'; // theme css file
import * as rdrLocales from 'react-date-range/dist/locale';
import {DateRange} from 'react-date-range';
import {addDays} from 'date-fns';
import {useSelector} from 'react-redux';
import {Form} from "rsuite";

const URL = `${process.env.REACT_APP_AXIOS_URL}`;

const GET_SCHOOLS_URL = `${URL}/user/schools`;
const GET_PRACTICE_LIST_URL = `${URL}/student/passed-practices-list`;
const GET_SUBJECTS_URL = `${URL}/user/subjects`;
const GET_TEACHERS_URL = `${URL}/user/teachers`;

export const PastPracticeListComponent = () => {
        const schoolNotFound = "Škola nevyplněna";
        const subjectNotFound = "Předmět nevyplněn";
        const schoolFilterParam = "School";
        const subjectFilterParam = "Subject";
        const teacherFilterParam = "Teacher";
        const dateRangeFilterParam = "Date";
        const allFilterParam = "All";
        const [modalShow, setModalShow] = React.useState(false);
        const review = useState("");
        const [currentPracticeId, setCurrentPracticeId] = useState("");

        let iconStyles = {fontSize: "1.5em", marginRight: "5px"};
        let iconStyleFilter = {fontSize: "1.5em", marginRight: "15px"};
        const [showing, setShowing] = useState(false);
        const [practices, setPraxe] = useState([]);
        const [filterParam, setFilterParam] = useState([allFilterParam]);
        const [schools, setSchools] = useState([]);
        const [teachers, setTeachers] = useState([]);
        const [subjects, setSubjects] = useState([]);
        const [dateLimit, setDateLimit] = useState([addDays(new Date(), -30), addDays(new Date(), 30)]);
        const noteNotFound = "Poznámka nevyplněna.";
        const [modalShowMyReview, setModalShowMyReview] = React.useState(false);

        const maxReviewLength = 1000;

        const [errorMessage, setErrorMessage] = useState("");
        const [successMessage, setSuccessMessage] = useState("");

        const pastPracticesRedux = useSelector((state) => state.practices);

        const [selectedSchool, setSelectedSchools] = useState("");
        const [selectedSubjectName, setSelectedSubjectName] = useState("");
        const [selectedTeacherName, setSelectedTeacherName] = useState("");
        const [btnText, setBtnText] = useState("Zobrazit možnosti vyhledávání");
        const [reviewText, setReviewText] = useState("")
        const [dateRange, setDateRange] = useState([
            {
                startDate: new Date(),
                endDate: new Date(),
                key: 'selection'
            }
        ]);

        const changeBtnText = () => {
            if (!showing) {
                setBtnText("Schovat možnosti vyhledávání");
            } else {
                setBtnText("Zobrazit možnosti vyhledávání");
            }
        }

        function resetFilter() {
            setFilterParam([allFilterParam]);
            setSelectedSchools("");
            setSelectedSubjectName("");
            setSelectedTeacherName("");
            setDateRange([
                {
                    startDate: new Date(),
                    endDate: new Date(),
                    key: 'selection'
                }
            ]);
        }

        const getPraxe = async () => {
            const response = await axios({
                url: GET_PRACTICE_LIST_URL,
                headers: { 'Authorization': localStorage.getItem("token") },
                withCredentials: true,
                method: "GET",
            }).catch((err) => {
                console.log(err.response.data.message);
            });
            if (response && response.data) {
                setPraxe(response.data);
                setDateRangeLimit(response.data);
            }
        };

        const postReview = async (practiceId, review) => {
            console.log("review text", review);
            const response = await axios({
                headers: {'content-type': 'application/json', 'Authorization': localStorage.getItem("token")},
                url: `${URL}/student/practices/${practiceId}/submitReview`,
                withCredentials: true,
                method: "POST",
                data: { text: review }
            }).catch((err) => {
                setErrorMessage(err.response.data.message);
                setSuccessMessage("");
            });
            if (response && response.data) {
                console.log(response.data);
                getPraxe();
            }
        };

        useEffect(() => {
            console.log("past practices use effect", pastPracticesRedux);
            getPraxe();
            getSchools();
            getSubjects();
            getTeachers();
        }, []);

        function setDateRangeLimit(practices) {
            let lowestDate = parseISO(practices[0].date);
            let highestDate = parseISO(practices[0].date);

            practices.forEach(element => {
                if (new Date(element.date.split('-')) < lowestDate) {
                    lowestDate = new Date(element.date.split('-'))
                }
                if (new Date(element.date.split('-')) > highestDate) {
                    highestDate = new Date(element.date.split('-'))
                }
            });
            setDateLimit([addDays(lowestDate, -1), addDays(highestDate, 1)]);
        }

        //create a createmodal function
        function CreateModalSubmitReview(props) {
            return (
                <Modal
                    {...props}
                    size="lg"
                    aria-labelledby="contained-modal-title-vcenter"
                    centered
                >
                    <Modal.Header closeButton>
                    </Modal.Header>
                    <Modal.Body>
                        <h4 className="mb-3">Přidání recenze</h4>
                        <form id="form_review" onSubmit={(e) => {
                            e.preventDefault();
                        }}>
                            <div className="mt-3 mb-3">
                                <div className="form-group">
                                <textarea rows="6" required maxLength={maxReviewLength} className="form-control"
                                          id="reviewTextArea"
                                          placeholder="Začněte psát recenzi" onChange={() => {
                                    review[0] = document.getElementById("reviewTextArea").value;
                                    document.getElementById("reviewTextArea").value = review[0];
                                    document.getElementById("review-len").innerText = `${review[0].length}/${maxReviewLength}`
                                }}/>
                                    <div className="float-end mb-3 pt-1" style={{marginRight: "5px"}}>
                                        <p id="review-len"><i>Zbývá znaků: {`${review[0].length}/${maxReviewLength}`}</i>
                                        </p>
                                    </div>
                                </div>
                                <div className="class-name pt-5"></div>
                            </div>
                            <div className="row float-end">
                                <div className="col d-flex move-storno-to-right" style={{padding: "0"}}>
                                    <button type="button" onClick={props.onHide}
                                            className="accept-btn my-btn-white">Storno
                                    </button>
                                </div>
                                <div className="col" style={{width: "200px"}}>
                                    <button type="button" form="form_review" className="review-btn my-hover-btn-send"
                                            onClick={() => {
                                                props.onHide();
                                                createReview();
                                            }}>Odeslat
                                    </button>
                                </div>
                            </div>
                        </form>
                    </Modal.Body>
                </Modal>
            );
        }

    function CreateModalMyReview(props) {
        return (
            <Modal
                {...props}
                size="lg"
                aria-labelledby="contained-modal-title-vcenter"
                centered
            >
                <Modal.Header closeButton>
                </Modal.Header>
                <Modal.Body>
                    <h4>Mé hodnocení</h4>
                    <div className="mt-3">
                    {reviewText ? <p>
                        {reviewText}
                    </p> : <p>Tuto praxi jste zatím neohodnotil...</p>}
                    </div>

                </Modal.Body>
                <Modal.Footer>
                    <button type="button" className="accept-btn my-btn-white" onClick={props.onHide}>Odejít</button>
                </Modal.Footer>
            </Modal>
        );
    }

        function createReview() {
            console.log("here");
            postReview(currentPracticeId,
                review[0]
            );
        }

        function search(items) {
            return items.filter((item) => {

                if (filterParam.includes(allFilterParam)) {
                    return true;
                }

                if (filterParam.includes(schoolFilterParam) && (item.teacher.school == null || item.teacher.school.name !== selectedSchool)) {
                    return false;
                }

                if (filterParam.includes(subjectFilterParam) && (item.subject == null || item.subject.name !== selectedSubjectName)) {
                    return false;
                }

                if (filterParam.includes(teacherFilterParam) && (item.teacher.firstName !== selectedTeacherName.split(" ")[0] || item.teacher.secondName !== selectedTeacherName.split(" ")[1])) {
                    return false;
                }

                if (filterParam.includes(dateRangeFilterParam) && (new Date(item.date.split('-')) < dateRange[0].startDate || new Date(item.date.split('-')) > dateRange[0].endDate)) {
                    return false;
                }
                return true;
            });
        }

        const getSchools = async () => {
            const response = await axios({
                headers: { 'Authorization': localStorage.getItem("token") },
                url: GET_SCHOOLS_URL,
                withCredentials: true,
                method: "GET",
            }).then((response) => {
                const sch = [];
                response.data.forEach(element => sch.push(element.name));
                setSchools(sch);
            });
        };

        const getSubjects = async () => {
            const response = await axios({
                headers: { 'Authorization': localStorage.getItem("token") },
                url: GET_SUBJECTS_URL,
                withCredentials: true,
                method: "GET",
            }).then((response) => {
                const sch = [];
                response.data.forEach(element => sch.push(element.name));
                setSubjects(sch);

            });
        };

        const getTeachers = async () => {
            const response = await axios({
                headers: { 'Authorization': localStorage.getItem("token") },
                url: GET_TEACHERS_URL,
                withCredentials: true,
                method: "GET",
            }).then((response) => {
                const sch = [];
                let res =
                    response.data.forEach(element => {
                        let str = element.firstName.concat(" ", element.secondName);
                        sch.push(str)
                    });
                setTeachers(sch);
            });
        };

        const selectSchoolsChange = (value) => {
            const index = filterParam.indexOf(allFilterParam);
            if (index > -1) {
                filterParam.splice(index, 1);
            }
            if (!filterParam.includes(schoolFilterParam)) {
                filterParam.push("School")
            }
            setSelectedSchools(value);
        }

        const selectSubjectChange = (value) => {
            const index = filterParam.indexOf(allFilterParam);
            if (index > -1) {
                filterParam.splice(index, 1);
            }
            if (!filterParam.includes(subjectFilterParam)) {
                filterParam.push(subjectFilterParam)
            }
            setSelectedSubjectName(value);
        }

        const selectTeacherChange = (value) => {
            const index = filterParam.indexOf(allFilterParam);
            if (index > -1) {
                filterParam.splice(index, 1);
            }
            if (!filterParam.includes(teacherFilterParam)) {
                filterParam.push(teacherFilterParam)
            }
            setSelectedTeacherName(value);
        }

        const selectDateRange = (ranges) => {
            const index = filterParam.indexOf(allFilterParam);
            if (index > -1) {
                filterParam.splice(index, 1);
            }
            if (!filterParam.includes(dateRangeFilterParam)) {
                filterParam.push(dateRangeFilterParam)
            }
            ranges.selection.endDate.setHours(23, 59, 59);
            setDateRange([ranges.selection]);
        }
        return (
            <Container fluid className="mb-3">
                <div>
                    <button id="toggleBtn" className="toggleButtonFilters" onClick={() => {
                        setShowing(!showing);
                        changeBtnText();
                    }}><BsSearch style={iconStyles}/> {btnText}</button>
                    <div style={{overflow: 'hidden'}}>
                        <div className={!showing ? 'hideDiv' : 'calendarDivHeight'}>
                            <div className="customFilters">
                                <div className="col align-self-center">
                                    <div className="align-self-center search-school">
                                        <p>Vyberte školu</p>
                                        <Combobox
                                            data={schools}
                                            value={selectedSchool}
                                            onChange={value => selectSchoolsChange(value)}
                                        />
                                    </div>
                                    <div className="align-self-center search-school">
                                        <p>Vyberte předmět</p>
                                        <Combobox
                                            data={subjects}
                                            value={selectedSubjectName}
                                            onChange={value => selectSubjectChange(value)}
                                        />
                                    </div>
                                    <div className="align-self-center search-school">
                                        <p>Vyberte učitele</p>
                                        <Combobox
                                            data={teachers}
                                            value={selectedTeacherName}
                                            onChange={value => selectTeacherChange(value)}
                                        />
                                    </div>
                                </div>
                                <div className="col align-self-center search-date">
                                    <p>Vyberte datum (od - do)</p>
                                    <DateRange
                                        editableDateInputs={true}
                                        onChange={item => selectDateRange(item)}
                                        moveRangeOnFirstSelection={false}
                                        ranges={dateRange}
                                        locale={rdrLocales.cs}
                                        minDate={dateLimit[0]}
                                        maxDate={dateLimit[1]}
                                    />
                                </div>
                            </div>
                            <div className="center">
                                <button id="filterResetBtn" className="filterResetBtn" onClick={() => {
                                    resetFilter();
                                }}><BsFillXCircleFill style={iconStyles}/> Reset
                                </button>
                            </div>
                        </div>
                    </div>
                    <hr/>
                </div>
                {!filterParam.includes(allFilterParam) && <div className="customAlertContainer">
                    <div className="p-3 m-3 center my-alert-filter alert-danger alertCustom">
                        <BsSliders style={iconStyleFilter}/>
                        <span><b>Filtr je aktivní</b></span>
                    </div>
                </div>}
                <Accordion>
                    <div style={{width: "100%"}}>
                        <div className="title-container text-info-practice">
                            <Row style={{width: "100%"}}>
                                <Col className="text-center">
                                    <b>Předmět</b>
                                </Col>
                                <Col className="text-center d-none">
                                    <b>Učitel</b>
                                </Col>
                                <Col className="text-center d-none d-xl-block">
                                    <b>Škola</b>
                                </Col>
                                <Col className="text-center">
                                    <b>Datum</b>
                                </Col>
                                <Col className="text-center d-none">
                                    <b>Čas</b>
                                </Col>
                                <Col className="text-center d-none">
                                    <b>E-mail</b>
                                </Col>
                                <Col className="text-center d-none">
                                    <b>Kapacita</b>
                                    <OverlayTrigger
                                        overlay={
                                            <Tooltip>
                                                Počet aktuálně zapsaných studentů / maximální počet
                                                studentů na praxi.
                                            </Tooltip>
                                        }
                                    >
                                    <span>
                                        <BsInfoCircleFill className={"info-tooltip mb-1"}/>
                                    </span>
                                    </OverlayTrigger>
                                </Col>
                            </Row>
                        </div>
                    </div>
                    {search(practices).length === 0 ?
                        <div className="alert alert-danger center warnTextPractices"><span>Nebyly nalezeny žádné praxe odpovídající zadaným parametrům.</span>
                        </div> : null}
                    {practices && search(practices).map((item, index) => (
                        <Accordion.Item
                            eventKey={item.id}
                            key={index}
                            style={{display: "block"}}
                        >
                            <div style={{display: "flex"}}>
                                <Accordion.Header className={"accordion-header-past-practices"}>
                                    <Row style={{width: "100%"}}>
                                        <Col
                                            className="text-center  ">{item.subject != null ? item.subject.name : subjectNotFound}</Col>
                                        <Col className="text-center d-none">
                                            {item.teacher.firstName + " " + item.teacher.secondName}
                                        </Col>
                                        <Col
                                            className="text-center d-none d-xl-block">{item.teacher.school != null ? item.teacher.school.name : schoolNotFound}</Col>
                                        <Col className="text-center">
                                        {format(parseISO(item.date), 'dd. MM. yyyy')}
                                        </Col>
                                        <Col className="text-center d-none">
                                            {item.start.split(":")[0] +
                                            ":" +
                                            item.start.split(":")[1] +
                                            " - " +
                                            item.end.split(":")[0] +
                                            ":" +
                                            item.end.split(":")[1]}
                                        </Col>
                                        <Col className="text-center d-none">
                                            {item.teacher.username}
                                        </Col>
                                        <Col className="text-center badge d-none">
                                            <div>
                                                <Badge
                                                    bg={
                                                        item.numberOfReservedStudents < item.capacity - 1
                                                            ? "success"
                                                            : "danger"
                                                    }
                                                >
                                                    {item.numberOfReservedStudents} / {item.capacity}
                                                </Badge>
                                            </div>
                                        </Col>
                                    </Row>
                                </Accordion.Header>
                            </div>

                            <Accordion.Body>
                                <div className="row listed-practices-row">
                                    <hr/>
                                    <div className="col col-in-practices">
                                        <p><b>Učitel:</b> {item.teacher.firstName + " " + item.teacher.secondName}</p>
                                        <p><b>E-mail:</b> {item.teacher.username}</p>
                                        <p><b>Čas: </b>
                                            <span>
                                            {item.start.split(":")[0] +
                                            ":" +
                                            item.start.split(":")[1] +
                                            " - " +
                                            item.end.split(":")[0] +
                                            ":" +
                                            item.end.split(":")[1]}</span></p>

                                        <b>Kapacita: </b>
                                        <span>
                                        <Badge
                                            bg={
                                                item.numberOfReservedStudents < item.capacity - 1
                                                    ? "success"
                                                    : "danger"
                                            }
                                        >
                                            {item.numberOfReservedStudents} / {item.capacity}
                                        </Badge>
                                    </span>

                                        <p style={{marginTop: "10px"}}><b>Poznámka:</b> {item.note != null ? item.note :
                                            <i>{noteNotFound}</i>}</p>

                                        <p style={{marginTop: "10px"}}><b>Soubory ke stažení:</b></p>
                                        <ul>
                                            {item.fileNames.length === 0 ?
                                                <p><i>Žádný soubor nebyl nahrán.</i></p>
                                                : ""}
                                            {item.fileNames.map((name, index) => (
                                                <li key={index}>
                                                    <a href={`${URL}/user/download/${item.teacher.username}/${name}`}>{name}</a>
                                                </li>
                                            ))
                                            }
                                        </ul>
                                    </div>
                                    <div className="center col div-cstm-flex-direction">
                                        <div className="mt-3 mb-1 flex-cont">
                                            <div className="center flex-it">
                                                <b>Report ke stažení: </b>
                                                <OverlayTrigger
                                                    overlay={
                                                        <Tooltip>
                                                            Toto uvidíte pouze vy, koordinátoři a student, který byl zapsán
                                                            na
                                                            tuto praxi.
                                                        </Tooltip>
                                                    }
                                                >
                                                <span>
                                                    <BsInfoCircleFill className={"info-tooltip mb-1"}/>
                                                </span>
                                                </OverlayTrigger>
                                            </div>
                                            <br/>
                                            {!item.report &&
                                            <span><i>Této praxi zatím nebyl přiřazen žádný report.</i></span>
                                            }
                                            <span className="d-inline-block text-truncate" style={{maxWidth: "300px"}}>
                                            {item.report &&
                                            <a className="report-dl"
                                               href={`${URL}/user/report/download/${item.id}`}><img
                                                src={DLImage}
                                                style={{
                                                    height: "30px",
                                                    marginRight: "5px",
                                                    textOverflow: 'ellipsis'
                                                }}
                                                alt={"DLImg"}/> {item.report}</a>}
                                        </span>
                                        </div>
                                        <hr className="w-75"/>
                                        {!item.reviews && <div className="d-flex align-items-center w-50">
                                            <button onClick={() => {
                                                setModalShow(true);
                                                setCurrentPracticeId(item.id)
                                            }}
                                                    className="btn toggleButtonFilters my-hover-btn-send">Přidat recenzi
                                            </button>
                                        </div>}
                                        {item.reviews && <div className="d-flex align-items-center">
                                            <br/>
                                                <button className="review-btn review-show-btn-student"
                                                        onClick={() => {setModalShowMyReview(true); setReviewText(item.reviews[0].reviewText)}}>Mé hodnocení
                                                </button>
                                        </div>}

                                    </div>
                                </div>
                            </Accordion.Body>
                        </Accordion.Item>
                    ))}
                </Accordion>
                <CreateModalSubmitReview
                    show={modalShow}
                    onHide={() => setModalShow(false)}
                />
                <CreateModalMyReview
                    show={modalShowMyReview}
                    onHide={() => setModalShowMyReview(false)}
                />
            </Container>
        );
    }
;

export default PastPracticeListComponent;
