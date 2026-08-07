{
  /*شروع از بخش نمونه کارهای ما*/
}
{
  /*نمونه‌کارها / نمایش کارهایی که انجام داده‌ای*/
}

import { motion } from "framer-motion";
import Container from "./ui/Container";

export default function Portfolio() {
  const projects = [
    {
      title: "فروشگاه اینترنتی آریا",
      category: "طراحی سایت + SEO",
      result: "+120% افزایش فروش",
    },
    {
      title: "برند دیجیتال نوین",
      category: "برندینگ + محتوا",
      result: "+80% افزایش تعامل",
    },
    {
      title: "پلتفرم آموزشی",
      category: "طراحی محصول",
      result: "+50K کاربر",
    },
  ];

  return (
    <motion.section
      initial={{
        opacity: 0,
        y: 50,
      }}
      whileInView={{
        opacity: 1,
        y: 0,
      }}
      viewport={{
        once: true,
      }}
      transition={{
        duration: 0.6,
      }}
      className="
            py-24
            "
    >
      <Container>
        <div
          className="
                text-center
                "
        >
          <h2
            className="
                    text-4xl
                    font-black
                    "
          >
            نمونه کارهای ما
          </h2>

          <p
            className="
                    mt-5
                    text-gray-600
                    "
          >
            پروژه‌هایی که با استراتژی درست رشد کرده‌اند
          </p>
        </div>

        <div
          className="
                mt-14
                grid
                md:grid-cols-3
                gap-8
                "
        >
          {projects.map((project, index) => (
            <motion.div
              key={project.title}
              initial={{
                opacity: 0,
                y: 40,
              }}
              whileInView={{
                opacity: 1,
                y: 0,
              }}
              viewport={{
                once: true,
              }}
              transition={{
                delay: index * 0.15,
                duration: 0.5,
              }}
              whileHover={{
                y: -6,
                scale: 1.05,
              }}
              className="
                        rounded-3xl
                        overflow-hidden
                        border
                        bg-white
                        hover:shadow-2xl
                        transition
                        "
            >
              {/* Project Image */}

              <div
                className="
                            h-56
                            bg-gradient-to-br
                            from-purple-600
                            to-green-400
                            flex
                            items-center
                            justify-center
                            text-white
                            text-3xl
                            font-bold
                            "
              >
                Preview
              </div>

              <div
                className="
                            p-8
                            "
              >
                <h3
                  className="
                                text-xl
                                font-bold
                                "
                >
                  {project.title}
                </h3>

                <p
                  className="
                                mt-3
                                text-purple-600
                                font-medium
                                "
                >
                  {project.category}
                </p>

                <div
                  className="
                                mt-5
                                bg-green-50
                                text-green-700
                                rounded-xl
                                px-4
                                py-3
                                text-sm
                                "
                >
                  {project.result}
                </div>

                <button
                  className="
                                mt-6
                                text-purple-600
                                font-bold
                                "
                >
                  مشاهده پروژه →
                </button>
              </div>
            </motion.div>
          ))}
        </div>
      </Container>
    </motion.section>
  );
}
